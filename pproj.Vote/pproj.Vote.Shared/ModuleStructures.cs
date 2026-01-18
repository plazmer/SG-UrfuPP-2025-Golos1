using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace pproj.Vote.Structures.Module
{

  /// <summary>
  /// Дто-модель для получения статистики голосов
  /// </summary>
  [Public]
  partial class PollStatisticDto
  {
    /// <summary>
    /// Идентификатор опроса
    /// </summary>
    public long PollVoteId {get;set;}
    
    /// <summary>
    /// Кол-во проголосовавших в опросе
    /// </summary>
    public int VotedCount {get;set;}
    
    /// <summary>
    /// Варианты ответа опроса с результатами
    /// </summary>
    public List<pproj.Vote.Structures.Module.IPollOptionStatisticDto> Options { get; set; }
  }
  
  /// <summary>
  /// Дто-модель статистики варианта ответа
  /// </summary>
  [Public]
  partial class PollOptionStatisticDto
  {
    /// <summary>
    /// Идентификатор варинта ответа
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Текст варианта
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Количество голосов
    /// </summary>
    public int? VotesCount { get; set; }
    
    /// <summary>
    /// Список проголосовавших за этот вариант ответа
    /// </summary>
    public List<long> EmployeesIds {get;set;}
  }
  
  
  // Создание опроса
  
  
  /// <summary>
  /// Дто-модель создания опроса/настройка
  /// </summary>
  [Public]
  partial class SurveyCreateDto
  {
    /// <summary>
    /// Название опроса
    /// </summary>
    public string SurveyName { get; set; }
    
    /// <summary>
    /// Описание опроса
    /// </summary>
    public string Description { get; set;}
    
    /// <summary>
    /// Анонимный опрос
    /// </summary>
    public bool IsAnonymous { get; set; }
    
    /// <summary>
    /// Перемешать вопросы
    /// </summary>
    public bool IsMix { get; set; }
    
    /// <summary>
    /// Показывать прогресс
    /// </summary>
    public bool IsShowProgress { get; set; }
    
    /// <summary>
    /// Вопрос
    /// </summary>
    public List<pproj.Vote.Structures.Module.IPollCreateDto> PollCreateDto { get; set; }
  }
  
    /// <summary>
  /// Дто-модель для создания опроса
  /// </summary>
  [Public]
  partial class PollCreateDto
  {
    /// <summary>
    /// Вопрос
    /// </summary>
    public string QuestionText { get; set; }
    
    /// <summary>
    /// Разрешён ли выбор нескольких вариантов
    /// </summary>
    public bool IsMultipleChoice { get; set; }

    /// <summary>
    /// Тексты вариантов ответа
    /// </summary>
    public List<pproj.Vote.Structures.Module.IPollOptionCreateDto> Options { get; set; }
  }
  
  /// <summary>
  /// Дто-модель варианта опроса для создания опроса
  /// </summary>
  [Public]
  partial class PollOptionCreateDto
  {
    /// <summary>
    /// Текст варианта ответа
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Тип варианта
    /// </summary>
    public int Type { get; set; }
    
    /// <summary>
    /// Шкала мин. текст
    /// </summary>
    public string ScaleMinText { get; set; }
    
    /// <summary>
    /// Шкала макс. текст
    /// </summary>
    public string ScaleMaxText { get; set; }
    
    /// <summary>
    /// Минимальная шкала
    /// </summary>
    public int ScaleMin { get; set; }
    
    /// <summary>
    /// Максимальная шкала
    /// </summary>
    public int ScaleMax { get; set; }
  }
  
  
  //Получить опрос
  
  
  /// <summary>
  /// Дто-модель получения опроса
  /// </summary>
  [Public]
  partial class SurveyGetDto
  {
    /// <summary>
    /// Название опроса
    /// </summary>
    public string SurveyName { get; set; }
    
    /// <summary>
    /// Описание опроса
    /// </summary>
    public string Description { get; set;}
    
    /// <summary>
    /// Вопрос
    /// </summary>
    public List<pproj.Vote.Structures.Module.IPollGetDto> PollGetDto { get; set; }
  }
  
  /// <summary>
  /// Дто-модель получение вопроса
  /// </summary>
  [Public]
  partial class PollGetDto
  {
    /// <summary>
    /// Идентификатор вопроса (Poll).
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Вопрос
    /// </summary>
    public string QuestionText { get; set; }
    
    /// <summary>
    /// Разрешён ли выбор нескольких вариантов
    /// </summary>
    public bool IsMultipleChoice { get; set; }

    /// <summary>
    /// Варианты ответов
    /// </summary>
    public List<pproj.Vote.Structures.Module.IPollOptionGetDto> Options { get; set; }
  }
  
  /// <summary>
  /// Дто-модель получение варианта ответа
  /// </summary>
  [Public]
  partial class PollOptionGetDto
  {
    /// <summary>
    /// Идентификатор варианта ответа (PollOption).
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Текст варианта ответа
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Тип варианта
    /// </summary>
    public int Type { get; set; }
    
    /// <summary>
    /// Шкала мин. текст
    /// </summary>
    public string ScaleMinText { get; set; }
    
    /// <summary>
    /// Шкала макс. текст
    /// </summary>
    public string ScaleMaxText { get; set; }
    
    /// <summary>
    /// Минимальная шкала
    /// </summary>
    public int ScaleMin { get; set; }
    
    /// <summary>
    /// Максимальная шкала
    /// </summary>
    public int ScaleMax { get; set; }
  }
  
  /// <summary>
  /// DTO: проголосовать по опросу целиком.
  /// </summary>
  [Public]
  partial class SurveyVoteDto
  {
    /// <summary>
    /// Идентификатор опроса (Survey).
    /// </summary>
    public long SurveyId { get; set; }

    /// <summary>
    /// Ответы по вопросам (Poll).
    /// </summary>
    public List<pproj.Vote.Structures.Module.IPollVoteDto> Answers { get; set; }
  }

  /// <summary>
  /// DTO: ответ на конкретный вопрос.
  /// </summary>
  [Public]
  partial class PollVoteDto
  {
    /// <summary>
    /// Идентификатор вопроса (Poll).
    /// </summary>
    public long PollId { get; set; }

    /// <summary>
    /// Выбранные варианты ответа (каждый будет сохранён как отдельный PollVote).
    /// </summary>
    public List<long> OptionIds { get; set; }

    /// <summary>
    /// Ответ для шкалы (если выбран вариант типа Scale).
    /// </summary>
    public int? ScaleAnswer { get; set; }

    /// <summary>
    /// Развёрнутый ответ (если выбран вариант типа Detailed).
    /// </summary>
    public string DetailedAnswer { get; set; }
  }
  
}