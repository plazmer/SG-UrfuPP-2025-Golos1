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
    /// Создать опрос
    /// </summary>
    [Public(WebApiRequestType = RequestType.Post)]
    public virtual long SurveyCreate (Structures.Module.ISurveyCreateDto surveyDto)
    {
      // Создание оболочки опроса с настройками 
      var survey = Surveys.Create();
      
      survey.SurveyName = surveyDto.SurveyName;
      survey.Description = surveyDto.Description;
      survey.IsAnonymous = surveyDto.IsAnonymous;
      survey.IsMix = surveyDto.IsMix;
      survey.IsShowProgress = surveyDto.IsShowProgress;
      survey.Author = Users.Current;
      
      survey.Save();
      
      // Создание вопроса - оболочки для вариантов ответа
      foreach (var pollDto in surveyDto.PollCreateDto)
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
      
      // Формирование модели вопросов
      
      return surveyGetDto;
    }
  }
}