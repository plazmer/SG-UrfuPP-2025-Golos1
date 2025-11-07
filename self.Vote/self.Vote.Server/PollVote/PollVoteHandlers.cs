using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using self.Vote.PollVote;

namespace self.Vote
{
  partial class PollVoteServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      base.BeforeSave(e);

      if (_obj.Poll != null && _obj.Poll.Deadline.HasValue && _obj.Poll.Deadline < Calendar.Now)
        e.AddError("Голосование уже завершено.");
    
      if (_obj.Poll != null && _obj.Voter != null)
      {
        var already = PollVotes.GetAll()
          .Any(v => v.Poll.Id == _obj.Poll.Id && Equals(v.Voter, _obj.Voter) && v.Id != _obj.Id);
    
        if (already)
          e.AddError("Вы уже голосовали в этом опросе.");
      }
    }
  }
}