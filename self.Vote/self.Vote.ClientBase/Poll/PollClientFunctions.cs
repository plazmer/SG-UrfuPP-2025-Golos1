using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using self.Vote.Poll;

namespace self.Vote.Client
{
  partial class PollFunctions
  {
    /// <summary>
    /// Проголосовать
    /// </summary>       
    public virtual void Vote(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var options = self.Vote.PublicFunctions.Poll.GetOptions(_obj).ToList();
      if (!options.Any())
      {
        Dialogs.ShowMessage("У этого опроса нет вариантов.");
        return;
      }

      // выбираем строкой, чтобы не ругался на типы
      var items = options.Select(o => $"{o.Id}: {o.Text}").ToList();

      var dlg = Dialogs.CreateInputDialog("Голосование");
      var select = dlg.AddSelect("Вариант", true, items.First());
      
      // вот тут была ошибка
      select.From(items.ToArray());

      if (dlg.Show() != DialogButtons.Ok)
        return;

      var selected = select.Value;
      var idPart = selected.Split(':')[0].Trim();
      long optionId;
      if (!long.TryParse(idPart, out optionId))
      {
        Dialogs.ShowMessage("Не удалось определить вариант.");
        return;
      }

      self.Vote.PublicFunctions.Poll.VoteUser(_obj, optionId);

      Dialogs.ShowMessage("Голос учтён.");
    }

    public virtual void ShowResults(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var results = self.Vote.PublicFunctions.Poll.GetResults(_obj);
      var text = string.Join("\r\n", results.Select(r => $"{r.Key}: {r.Value}"));
      Dialogs.ShowMessage(text, "Результаты");
    }
  }
}