using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace pproj.Vote.Structures.Module
{

  /// <summary>
  /// Дто-модель для создания опроса
  /// </summary>
  [Public]
  partial class PollCreateDto
  {
    /// <summary>
    /// Тема опроса
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Описание / вопрос
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Разрешён ли выбор нескольких вариантов
    /// </summary>
    public bool IsMultipleChoice { get; set; }

    /// <summary>
    /// Тексты вариантов ответа
    /// </summary>
    public List<string> Options { get; set; }
  }
  
  /// <summary>
  /// Дто-модель опроса
  /// </summary>
  [Public]
  partial class PollDto
  {
    /// <summary>
    /// Идентификатор опроса
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Заголовок опроса
    /// </summary>
    public string Subject { get; set; }
    
    /// <summary>
    /// Описаниме опроса
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Один/Несколько вариантов ответа
    /// </summary>
    public bool IsMultipleChoice { get; set; }
    
    /// <summary>
    /// Статус опроса
    /// </summary>
    public string StatusVote { get; set; } // Draft / Active / Closed
    
    /// <summary>
    /// Варианты опроса
    /// </summary>
    public List<IPollOptionDto> Options { get; set; }
    
    /// <summary>
    /// Выбранные варианты для пользователя
    /// </summary>
    public long? MyVoteOptionId { get; set; } //TODO List<long>
  }

  /// <summary>
  /// Дто-модель варианта ответа
  /// </summary>
  [Public]
  partial class PollOptionDto
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
  }
}