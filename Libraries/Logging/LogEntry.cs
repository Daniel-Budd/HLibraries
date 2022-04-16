using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Libraries;
using static System.Console;
namespace Libraries.Logging
{
  public enum LogType
  {
    General = 0,
    Debug = 1,
    Event = 2
  }
  public class LoggingEntity
  {
    protected LogType logType { get; init; }
    protected List<LogEntry> entries = new List<LogEntry>();
    protected LoggingStyle loggingStyle { get; init; }
    protected EntryProfile PreSetProfile { get; init; }
    protected EntryType preSetType { get => PreSetProfile.Type; }
    protected EntrySubject preSetSubject { get => PreSetProfile.Subject; }
    protected EntryImportance preSetImportance { get => PreSetProfile.Importance; }
    internal LoggingEntity(LogType logtype, LoggingStyle loggingStyle_, EntryProfile p) 
    {
      logType = logtype;
      loggingStyle = loggingStyle_;
      PreSetProfile = p;
      OnEntryRecorded += (sender, args) => {};
    }
    public static LoggingEntity Create(LogType logType, LoggingStyle loggingStyle_, EntryProfile p) => new LoggingEntity(logType, loggingStyle_, p);
    /**
    <summary>Creates a <see cref="LoggingEntity"/> to log data.</summary>
    <param name="logType">The type of the log. Either General, Debug, or Event as of now.</param>
    <param name="loggingStyle">The style of log. Determines if the styles are provided or not. </param>
    <remarks>
      <para>
      </para>
      <example>For Example: <br/>
        <code>LoggingEntity.Create(loggingStyle: LoggingStyle: ProvideAll, Importance: EntryImportance.Fatal, Subject: EntrySubject.Loophole | EntrySubject.Execution, Type: EntryType: EntryType.Suggestion)</code> <br/>
        Would create a log that predetermines that all messages are either execution flaws or loopholes.
      </example>
    </remarks>
    */
    public static LoggingEntity Create([Optional]LogType logType, [Optional]LoggingStyle loggingStyle, [Optional]EntryImportance Importance, [Optional]EntrySubject Subject, [Optional]EntryType Type)
    {
      return new LoggingEntity(logType, loggingStyle, new(Importance: Importance, Subject: Subject, Type: Type));
    }
    public void Display()
    {
      entries.Sort();
      Console.Clear();
      Console.SetCursorPosition(0,0);
      Console.WriteLine(DateTime.Now);
      foreach(LogEntry entry in entries)
      {
        Show(entry);
      }
    }
    public async Task DisplayAsync()
    {
      entries.Sort();
      Console.Clear();
      await Task.Run(() => Console.SetCursorPosition(0,0));
      await Task.Run(() => Console.WriteLine(DateTime.Now));
      foreach(LogEntry entry in entries)
      {
        await Task.Run(() => Show(entry));
      }
    }
    protected void Show(LogEntry entry)
    {
      switch(entry.Type)
      {
        case EntryType.Warning:
          ForegroundColor = ConsoleColor.DarkYellow;
          break;
        case EntryType.Error:
          ForegroundColor = ConsoleColor.DarkRed;
          break;
        case EntryType.Information:
          ForegroundColor = ConsoleColor.Gray;
          break;
        case EntryType.Suggestion:
          ForegroundColor = ConsoleColor.Cyan;
          break;
        case EntryType.Logging:
          ForegroundColor = ConsoleColor.DarkGreen;
          break;
      }
      WriteLine($"{logType} Log Recorded {entry.Importance} Priority {entry.Subject} Message: {entry.message}");
      WriteLine("Extra Data: ");
      foreach(var e in entry.data)
      {
        WriteLine(e);
      }
      ResetColor();
    }
    protected async Task ShowAsync(LogEntry entry)
    {
      await Task.Run(() => {switch(entry.Type)
      {
        case EntryType.Warning:
          ForegroundColor = ConsoleColor.DarkYellow;
          break;
        case EntryType.Error:
          ForegroundColor = ConsoleColor.DarkRed;
          break;
        case EntryType.Information:
          ForegroundColor = ConsoleColor.Gray;
          break;
        case EntryType.Suggestion:
          ForegroundColor = ConsoleColor.Cyan;
          break;
        case EntryType.Logging:
          ForegroundColor = ConsoleColor.DarkGreen;
          break;
      }});
      await Task.Run(() => WriteLine($"{logType} Log Recorded {entry.Importance} Priority {entry.Subject} Message: {entry.message}"));
      await Task.Run(() => WriteLine("Extra Data: "));
      foreach(var e in entry.data)
      {
        await Task.Run(() => Console.WriteLine(e));
      }
      await Task.Run(() => ResetColor());
    }
    public void RecordEntry(string message, MetaData data)
    {
      EntryProfile profile = new(Importance:preSetImportance, Subject: preSetSubject, Type: preSetType);
      OnEntryRecorded(this, new LogEntryRecordedEventArgs(logType, new LogEntry(message, profile, data)));
    }
    public void RecordEntry(string message, EntryProfile profile_, params MetaDataEntry[] metadata)
    {
      bool providesEnums = (loggingStyle & LoggingStyle.ProvideEnums) != 0;
      EntryProfile profile = providesEnums ? PreSetProfile : profile_;
      OnEntryRecorded(this, new LogEntryRecordedEventArgs(logType, new LogEntry(message, profile, metadata)));
    }
    public void RecordEntry(string message, [Optional]EntryImportance Importance, [Optional]EntrySubject Subject, [Optional]EntryType Type)
    {
      bool providesEnums = (loggingStyle & LoggingStyle.ProvideEnums) != 0;
      EntryProfile profile = providesEnums ? PreSetProfile : new(Importance: Importance, Subject: Subject, Type: Type);
      OnEntryRecorded(this, new LogEntryRecordedEventArgs(logType, new LogEntry(message, profile)));
    }
    public event EventHandler<LogEntryRecordedEventArgs> OnEntryRecorded;
    // public void Record(string message, )
  }
  public class LogEntryRecordedEventArgs : EventArgs
  {
    public LogType logType { get; init; }
    public LogEntry Entry { get; init; }
    public LogEntryRecordedEventArgs(LogType type, LogEntry entry) : base()
    {
      logType = type;
      Entry = entry;
    }
    ~LogEntryRecordedEventArgs()
    {
      // 
    }
    public void Deconstruct()
    {
      // 
    }
    public void Deconstruct(out LogType type)
    {
      type = logType;
    }
    public void Deconstruct(out LogType type,out LogEntry entry)
    {
      type = logType;
      entry = Entry;
    }
  }
  public static class Loggers
  {
    private static LoggingEntity _debug = new LoggingEntity(LogType.Debug, LoggingStyle.ProvideAll, new(EntryImportance.Average, EntrySubject.Warning,  EntryType.Warning));
    public static LoggingEntity Debug { get => _debug; }
    private static LoggingEntity _events = new LoggingEntity(LogType.Event, LoggingStyle.ProvideData, new(EntryImportance.Minimal, EntrySubject.Event, EntryType.Logging));
    public static LoggingEntity Events { get => _events; }
    private static LoggingEntity _general = new LoggingEntity(LogType.General, LoggingStyle.ProvideNone, new(EntryImportance.None, EntrySubject.General, EntryType.Information));
    public static LoggingEntity GeneralLog { get => _general; }
  }
  public record struct EntryProfile(EntryImportance Importance, EntrySubject Subject, EntryType Type)
  {
    public EntryImportance Importance { get; init; } = Importance;
    public EntrySubject Subject { get; init; } = Subject;
    public EntryType Type { get; init; } = Type;
    public static readonly EntryProfile GeneralDefault = new EntryProfile(EntryImportance.Average, EntrySubject.General, EntryType.Information);
    public static readonly EntryProfile ErrorDefault = new EntryProfile(EntryImportance.Fatal, EntrySubject.Warning | EntrySubject.Error, EntryType.Error);
    public static implicit operator EntryProfile((EntryImportance imp, EntrySubject sub, EntryType typ) setup) => new(Importance: setup.imp, Subject: setup.sub, Type: setup.typ);
  }
  [Flags]
  public enum LoggingStyle
  {
    ProvideAll = 0b11,
    ProvideEnums=0b10,
    ProvideData =0b01,
    ProvideNone =0b00
  }
  public enum EntryImportance
  {
    None = 0,
    Minimal = 1,
    Low = 2,
    Average = 3,
    High = 4,
    Fatal = 5
  }
  public enum EntryType
  {
    Information = 0,
    Suggestion = 2,
    Warning = 3,
    Error = 4,
    Logging = 5
  }
  [Flags]
  public enum EntrySubject
  {
    General =   0b0000000000000000,
    Execution = 0b0000000000000001,
    Loophole  = 0b0000000000000010,
    Error     = 0b0000000000000100,
    Warning   = 0b0000000000001000,
    Conversion= 0b0000000000010000,
    Event     = 0b0000000000100000,
    Tracker   = 0b0000000001000000,
    Metadata  = 0b0000000010000000,
    Other     = 0b0000000100000000,
  }
  public struct LogEntry : IComparable<LogEntry>
  {
    public EntryProfile Profile { get; init; }
    public string message { get; init; }
    public MetaData data { get; init; }
    private LogEntry(LogEntry entry, EntrySubject subject_)
    {
      message = entry.message;
      data = entry.data;
      Profile = new(Importance: entry.Importance, Subject: subject_, Type: entry.Type);
    }
    private LogEntry(LogEntry entry, EntryImportance importance_)
    {
      message = entry.message;
      data = entry.data;
      Profile = new(Importance: importance_, Subject: entry.Subject, Type: entry.Type);
    }
    private LogEntry(LogEntry entry, EntryType type_)
    {
      message = entry.message;
      data = entry.data;
      Profile = new(Importance: entry.Importance, Subject: entry.Subject, Type: type_);
    }
    private LogEntry(LogEntry entry, EntryProfile profile)
    {
      message = entry.message;
      data = entry.data;
      Profile = profile;
    }
    public LogEntry(string message_, params MetaDataEntry[] metaDataEntries)
    {
      data = metaDataEntries;
      message = message_;
      Profile = new(Importance: EntryImportance.Average, Subject: EntrySubject.General, Type: EntryType.Information);
    }
    public LogEntry(string message_, MetaData data_)
    {
      data = data_;
      message = message_;
      Profile = new(Importance: EntryImportance.Average, Subject: EntrySubject.General, Type: EntryType.Information);
    }
    public LogEntry(string message_, EntryProfile profile, params MetaDataEntry[] metaDataEntries)
    {
      data = metaDataEntries;
      message = message_;
      Profile = new(Importance: EntryImportance.Average, Subject: EntrySubject.General, Type: EntryType.Information);
    }
    public LogEntry(string message_, EntryProfile profile, MetaData data_)
    {
      data = data_;
      message = message_;
      Profile = new(Importance: EntryImportance.Average, Subject: EntrySubject.General, Type: EntryType.Information);
    }
    public EntryType Type { get => Profile.Type; }
    public EntryImportance Importance { get => Profile.Importance; }
    public EntrySubject Subject { get => Profile.Subject; } 
    public LogEntry WithType(EntryType type_) => new LogEntry(this,type_);
    public LogEntry WithSubject(EntrySubject subject_) => new LogEntry(this,subject_);
    public LogEntry WithImportance(EntryImportance importance_) => new LogEntry(this,importance_);
    public LogEntry WithProfile(EntryProfile profile) => new LogEntry(this, profile);
    public int CompareTo(LogEntry other)
    {
      int imp = Importance.CompareTo(other.Importance);
      int ent = Type.CompareTo(other.Type);
      int sub = Subject.CompareTo(other.Subject);
      int mes = message.CompareTo(other.message);
      int dat = data.CompareTo(other.data);
      return imp == 0 ? ent == 0 ? sub == 0 ? mes == 0 ? dat : mes : sub : ent: imp;
    }
    // public LogEntry()
  }
}