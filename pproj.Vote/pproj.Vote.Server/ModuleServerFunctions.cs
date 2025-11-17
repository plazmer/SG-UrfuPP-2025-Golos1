using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace pproj.Vote.Server
{
  public partial class ModuleFunctions
  {
    /// <summary>
    /// Интеграционная функция: получить опрос.
    /// </summary>
    /// <param name="pollId">ИД опроса.</param>
    [Public(WebApiRequestType = RequestType.Get)]
    public virtual Structures.Module.IPollDto GetPoll(long pollId)
    {
      var poll = Polls.GetAll(p => p.Id == pollId).FirstOrDefault();
      if (poll == null)
        return null;
    
      if (poll.StatusVote == pproj.Vote.Poll.StatusVote.Draft)
        return null;
    
      var employee = Sungero.Company.Employees.Current;
    
      var optionDtos = PollOptions
        .GetAll(o => Equals(o.Poll, poll))
        .OrderBy(o => o.Order)
        .Select(o => (Structures.Module.IPollOptionDto)
          Structures.Module.PollOptionDto.Create(
            o.Id,
            o.Text,
            null
          ))
        .ToList();
    
      var myVote = PollVotes
        .GetAll(v => Equals(v.Poll, poll) && Equals(v.Employee, employee))
        .FirstOrDefault();
    
      var pollDto = Structures.Module.PollDto.Create();
      pollDto.Id = poll.Id;
      pollDto.Subject = poll.Subject;
      pollDto.Description = poll.Description;
      pollDto.IsMultipleChoice = poll.IsMultipleChoice ?? false;
      pollDto.StatusVote = poll.StatusVote.ToString();
      pollDto.Options = optionDtos;
      pollDto.MyVoteOptionId = myVote != null ? (long?)myVote.Option.Id : null;
    
      return pollDto;
    }
    
    /// <summary>
    /// Создать опрос.
    /// </summary>
    /// <param name="pollDto">Данные для создания опроса.</param>
    /// <returns>Id созданного опроса.</returns>
    [Public(WebApiRequestType = RequestType.Post)]
    public virtual long PollCreate(Structures.Module.IPollCreateDto pollDto)
    {
      if (pollDto == null)
        throw new ArgumentNullException(nameof(pollDto));

      if (string.IsNullOrWhiteSpace(pollDto.Subject))
        throw new ArgumentException("Subject is required.", nameof(pollDto));

      if (pollDto.Options == null || !pollDto.Options.Any())
        throw new ArgumentException("At least one option is required.", nameof(pollDto));

      var poll = Polls.Create();
      poll.Subject = pollDto.Subject;
      poll.Description = pollDto.Description;
      poll.IsMultipleChoice = pollDto.IsMultipleChoice;
      poll.Save();

      var order = 1;
      foreach (var optionText in pollDto.Options.Where(t => !string.IsNullOrWhiteSpace(t)))
      {
        var option = PollOptions.Create();
        option.Poll = poll;
        option.Text = optionText;
        option.Save();
        order++;
      }

      return poll.Id;
    }
  }
}