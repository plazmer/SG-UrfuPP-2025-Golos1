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
    /// Тема опроса.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Описание / вопрос.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Разрешён ли выбор нескольких вариантов.
    /// </summary>
    public bool IsMultipleChoice { get; set; }

    /// <summary>
    /// Тексты вариантов ответа.
    /// </summary>
    public List<string> Options { get; set; }
  }
  
  /// <summary>
  /// Дто-модель опроса
  /// </summary>
  [Public]
  partial class PollDto
  {
    public long Id { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public bool IsMultipleChoice { get; set; }
    public string StatusVote { get; set; } // Draft / Active / Closed
    public System.Collections.Generic.List<pproj.Vote.Structures.Module.IPollOptionDto> Options { get; set; }
    public long? MyVoteOptionId { get; set; }
  }

  /// <summary>
  /// Дто-модель варианта ответа
  /// </summary>
  [Public]
  partial class PollOptionDto
  {
    public long Id { get; set; }
    public string Text { get; set; }
    public int? VotesCount { get; set; }
  }

}