using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using self.Vote.Poll;

namespace self.Vote.Server
{
  partial class PollFunctions
  {
    /// <summary>
    /// Получить все голоса
    /// </summary> 
    [Public]
    public virtual List<self.Vote.IPollOption> GetOptions()
    {
      return self.Vote.PollOptions.GetAll()
        .Where(o => o.Poll != null && o.Poll.Id == _obj.Id)
        .OrderBy(o => o.Id)
        .ToList();
    }

    // 2. проголосовать
    [Public]
    public virtual void VoteUser(long optionId)
    {
      var current = Sungero.Company.Employees.Current;

      if (_obj.Deadline.HasValue && _obj.Deadline < Calendar.Now)
        throw AppliedCodeException.Create("Голосование уже завершено.");

      var already = self.Vote.PollVotes.GetAll()
        .Any(v => v.Poll.Id == _obj.Id && Equals(v.Voter, current));
      if (already)
        throw AppliedCodeException.Create("Вы уже голосовали в этом опросе.");

      var option = self.Vote.PollOptions.GetAll()
        .FirstOrDefault(o => o.Id == optionId && o.Poll.Id == _obj.Id);
      if (option == null)
        throw AppliedCodeException.Create("Вариант ответа не найден.");

      var vote = self.Vote.PollVotes.Create();
      vote.Poll = _obj;
      vote.PollOption = option;      // имя подставь своё, если другое
      vote.Voter = current;
      vote.VotedAt = Calendar.Now;
      vote.Save();
    }

    // 3. результаты
    [Public]
    public virtual System.Collections.Generic.Dictionary<string, int> GetResults()
    {
      var votes = self.Vote.PollVotes.GetAll()
        .Where(v => v.Poll.Id == _obj.Id)
        .ToList();

      var options = self.Vote.PollOptions.GetAll()
        .Where(o => o.Poll.Id == _obj.Id)
        .ToList();

      var result = new System.Collections.Generic.Dictionary<string, int>();
      foreach (var option in options)
      {
        var count = votes.Count(v => v.PollOption != null && v.PollOption.Id == option.Id);
        result[option.Text] = count;
      }
      return result;
    }
  }
}