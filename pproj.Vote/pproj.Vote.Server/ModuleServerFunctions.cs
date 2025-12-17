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
    /// Получить статистику по опросу
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public virtual Structures.Module.IPollStatisticDto GetPollStatistic(long pollId)
    {
      var pollStatisticDto = Structures.Module.PollStatisticDto.Create();
      
      var voteCount = PollVotes.GetAll().Where(x => x.Poll.Id == pollId).Count();  
      var voteOptions = new List<pproj.Vote.Structures.Module.IPollOptionStatisticDto>();
      var votes = PollVotes.GetAll().Where(x => x.Poll.Id == pollId).GroupBy(x => x.Option.Id);
      
      foreach(var vote in votes)
      {
        var optionDto = Structures.Module.PollOptionStatisticDto.Create();
        
        optionDto.Id = vote.Key;
        optionDto.Text = PollOptions.GetAll().Where(x => x.Id == vote.Key).FirstOrDefault().Text;
        optionDto.VotesCount = vote.Count();
        optionDto.EmployeesIds = vote.Select(x => x.Employee.Id).Distinct().ToList();
        
        voteOptions.Add(optionDto);
      }
      
      pollStatisticDto.PollVoteId = pollId;
      pollStatisticDto.VotedCount = voteCount;
      pollStatisticDto.Options = voteOptions;
      
      return pollStatisticDto;
    }
    
    /// <summary>
    /// Создание черновика задачи
    /// </summary>
    [Public(WebApiRequestType = RequestType.Post)]
    public virtual long CreateDraftTaskFromSurvey(Structures.Module.ISurveyCreateDto surveyCreateDto)
    {
      if (surveyCreateDto == null) throw new ArgumentNullException(nameof(surveyCreateDto));
    
      var task = pproj.Vote.VoteTasks.Create();
      task.Subject = string.IsNullOrWhiteSpace(surveyCreateDto.SurveyName)
        ? "Опрос"
        : surveyCreateDto.SurveyName;
    
      task.Save();
    
      var surveyId = SurveyCreate(surveyCreateDto);
      var survey = Surveys.Get(surveyId);
    
      task.Survey = survey;
      task.Save();
    
      return task.Id;
    }

    /// <summary>
    /// Создать опрос
    /// </summary>
    [Public(WebApiRequestType = RequestType.Post)]
    public virtual long SurveyCreate (Structures.Module.ISurveyCreateDto surveyCreateDto)
    {
      // Создание оболочки опроса с настройками 
      var survey = Surveys.Create();
      
      survey.SurveyName = surveyCreateDto.SurveyName;
      survey.Description = surveyCreateDto.Description;
      survey.IsAnonymous = surveyCreateDto.IsAnonymous;
      survey.IsMix = surveyCreateDto.IsMix;
      survey.IsShowProgress = surveyCreateDto.IsShowProgress;
      survey.Author = Users.Current;
      
      survey.Save();
      
      // Создание вопроса - оболочки для вариантов ответа
      foreach (var pollDto in surveyCreateDto.PollCreateDto)
      {
        var poll = Polls.Create();
        
        poll.Survey = survey;
        poll.QuestionText = pollDto.QuestionText;
        poll.IsMultipleChoice = pollDto.IsMultipleChoice;
        
        poll.Save();
        
        // Создание варианта ответа
        foreach (var optionDto in pollDto.Options)
        {
          var option = PollOptions.Create();
          
          option.Poll = poll;
          option.Text = optionDto.Text;
          
          switch (optionDto.Type)
            {
              case 0:
                option.PollOptionType = pproj.Vote.PollOption.PollOptionType.Standard;
                break;
              case 1:
                option.PollOptionType = pproj.Vote.PollOption.PollOptionType.Scale;
                break;
              case 2:
                option.PollOptionType = pproj.Vote.PollOption.PollOptionType.Detailed;
                break;
              default:
                throw new InvalidOperationException($"Неизвестный тип варианта ответа: {optionDto.Type}");
            }
          
          if (optionDto.Type > 0)
          {
            option.ScaleMin = optionDto.ScaleMin;
            option.ScaleMax = optionDto.ScaleMax;
            option.ScaleMinText = optionDto.ScaleMinText;
            option.ScaleMaxText = optionDto.ScaleMaxText;
          }
          
          option.Save();
        }
      }
      
      return survey.Id;
    }
    
    
    /// <summary>
    /// Получить опрос
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public virtual Structures.Module.ISurveyGetDto GetSurvey (long surveyId)
    {
      // Формирование модели опроса
      var survey = Surveys.GetAll().FirstOrDefault(x => x.Id == surveyId);
      
      if (survey == null) throw new ArgumentException($"Survey not found id:{surveyId}");
      
      var surveyGetDto = Structures.Module.SurveyGetDto.Create();
      
      surveyGetDto.SurveyName = survey.SurveyName;
      surveyGetDto.Description = survey.Description;
      
      var polls = Polls.GetAll().Where(x => x.Survey.Id == surveyId);
      var pollList = new List<Structures.Module.IPollGetDto>();
      
      foreach (var poll in polls)
      {
        var pollGetDto = Structures.Module.PollGetDto.Create();
        pollGetDto.IsMultipleChoice = (bool)poll.IsMultipleChoice;
        pollGetDto.QuestionText = poll.QuestionText;
        
        var options = PollOptions.GetAll().Where(x => x.Poll.Id == poll.Id);
        var optionsList = new List<Structures.Module.IPollOptionGetDto>(); 
        
        foreach (var option in options)
        {
          var optionGetDto = Structures.Module.PollOptionGetDto.Create();
          
          var t = option.PollOptionType.Value;
          
          if (t == pproj.Vote.PollOption.PollOptionType.Standard)
            optionGetDto.Type = 0;
          else if (t == pproj.Vote.PollOption.PollOptionType.Scale)
            optionGetDto.Type = 1;
          else if (t == pproj.Vote.PollOption.PollOptionType.Detailed)
            optionGetDto.Type = 2;
          else
            throw new InvalidOperationException($"Неизвестный тип варианта ответа: {t}");
          
          optionGetDto.Text = option.Text;
          optionGetDto.ScaleMin = option.ScaleMin ?? 0;
          optionGetDto.ScaleMax = option.ScaleMax ?? 0;
          optionGetDto.ScaleMinText = option.ScaleMinText;
          optionGetDto.ScaleMaxText = option.ScaleMaxText;
          
          optionsList.Add(optionGetDto);
        }
        
        pollGetDto.Options = optionsList;
        pollList.Add(pollGetDto);
      }
      
      surveyGetDto.PollGetDto = pollList;
      
      // Формирование модели вопросов
      
      return surveyGetDto;
    }
    
    /// <summary>
    /// Получить опрос, привязанный к родительской задаче, по ID задания
    /// </summary>
    [Public(WebApiRequestType = RequestType.Get)]
    public virtual Structures.Module.ISurveyGetDto GetSurveyByAssignment(long assignmentId)
    {
      var assignment = Sungero.Workflow.Assignments.Get(assignmentId);
      if (assignment == null)
        throw new ArgumentException($"Assignment not found id:{assignmentId}");

      var task = assignment.Task;
      if (task == null)
        throw new InvalidOperationException("Assignment has no parent task.");

      var voteTask = pproj.Vote.VoteTasks.As(task);
      if (voteTask == null)
        throw new InvalidOperationException("Parent task is not VoteTask.");

      if (voteTask.Survey == null)
        throw new InvalidOperationException("Survey is not bound to the task.");

      return GetSurvey(voteTask.Survey.Id);
    }

  }
}