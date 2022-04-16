using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Console;
namespace Libraries;
public enum GeneralColor
{
  Transparent = 0,
  Black = 1,
  DarkBlue = 2,
  DarkGreen = 3,
  DarkCyan = 4,
  DarkRed = 5,
  DarkMagenta = 6,
  DarkYellow = 7,
  Gray = 8,
  DarkGray = 9,
  Blue = 10,
  Green = 11,
  Cyan = 12,
  Red = 13,
  Magenta = 14,
  Yellow = 15,
  White = 16
}
public interface IStyled
{
  public IStyled Invert();
  public IStyled AltHighlight([Optional]GeneralColor color);
  public IStyled Highlight([Optional]GeneralColor color);
  public IStyled WithStyle(ConsoleStyle style);
}
public struct ConsoleStyle : IComparable<ConsoleStyle>, IEquatable<ConsoleStyle>
{
  public GeneralColor Foreground { get; init; }
  public GeneralColor Background { get; init; }
  public ConsoleStyle(GeneralColor foreground = GeneralColor.DarkRed, GeneralColor background = GeneralColor.Transparent)
  {
    Foreground = foreground;
    Background = background;
  }
  public ConsoleStyle(ConsoleStyle style, [Optional]GeneralColor foreground, [Optional]GeneralColor background)
  {
    Foreground = foreground == GeneralColor.Transparent ? style.Foreground : foreground;
    Background = background == GeneralColor.Transparent ? style.Background : background;
  }
  internal void Setup()
  {
    ForegroundColor = Foreground.ToConsoleColor(true).NotNull();
    BackgroundColor = Background.ToConsoleColor(true).NotNull();
  }
  public ConsoleStyle Invert() => new(foreground: Background, background: Foreground);
  public ConsoleStyle AltHighlight(GeneralColor color) => Foreground != color ? new(this, background: color) : Invert();
  public ConsoleStyle Highlight(GeneralColor color) => new(this, background: color);
  public override int GetHashCode() => 1;
  public override bool Equals(object? obj)=>  obj == null ? false : obj is ConsoleStyle style ? this.Equals(style) : false;
  public bool Equals(ConsoleStyle style)
  {
    return Foreground == style.Foreground && Background == style.Background;
  }
  public int CompareTo(ConsoleStyle style)
  {
    return Foreground.CompareTo(style.Foreground);
  }
}
public class Application
{
  protected virtual void InitializeComponent()
  {
    //
  }
  protected Application()
  {
    InitializeComponent();
  }
}
/// <summary>
/// 
/// </summary>
public abstract class CustomApplication : Application, IKeyListener
{
  protected InputStream Input = InputStream.CreateNew();
  protected override abstract void InitializeComponent();
  protected CustomApplication() : base() {
    Input.Start();
  }
  public void Exit()
  {
    Input.Exit();
  }
  protected abstract void OnKeyPressed(object sender, KeyPressEventArgs args);
}
public interface IKeyListener
{
  protected void OnKeyPressed(object sender, KeyPressEventArgs args);
}
public abstract class BaseThread
{
  private Thread _thread;

  protected BaseThread()
  {
    _thread = new Thread(new ThreadStart(this.RunThread));
  }
  public void Start() => _thread.Start();
  public void Join() => _thread.Join();
  public bool IsAlive => _thread.IsAlive;
  protected abstract void RunThread();
}
public abstract class Component
{
  internal string ComponentID { get; init; }
  protected Component(string id)
  {
    ComponentID = id;
  }
  protected internal abstract void InitializeComponent();
}
public sealed class InputStream
{
  internal enum InputMethod
  {
    Line = 0,
    Key = 1
  }
  public static InputStream NewLineReader()
  {
    return new InputStream(InputMethod.Line);
  }
  public static InputStream NewKeyReader()
  {
    return new InputStream(InputMethod.Key);
  }
  private bool _started = false;
  private bool _done = false;
  public void Start() => _started = true;
  internal InputStream(InputMethod im) 
  {
    PreviousLine = "";
    OnKeyPress += (sender, args) => PreviousKeyPress = args.keyInfo;
    OnLineReceived += (sender, args) => PreviousLine = args.line;
  }
  public string PreviousLine { get; private set; }
  public ConsoleKeyInfo PreviousKeyPress { get; private set; }
  public ulong Time = 0L;
  internal void HandleKeyPress()
  {
    GetKey();
  }
  internal ConsoleKeyInfo GetKey([Optional]bool show)
  {
    ConsoleKeyInfo result = ReadKey(!show);
    OnKeyPress(this, new KeyPressEventArgs(result));
    return result;
  }
  public event EventHandler<KeyPressEventArgs> OnKeyPress;
  public event EventHandler<InputReceivedEventArgs> OnLineReceived;
  public void Exit() => _done = true;
}
public sealed class KeyPressEventArgs : EventArgs
{
  public ConsoleKeyInfo keyInfo { get; }
  public KeyPressEventArgs(ConsoleKeyInfo i) { keyInfo = i;}
}
public sealed class InputReceivedEventArgs : EventArgs
{
  public string line {get;}
  public InputReceivedEventArgs(string line_)
  {
    line = line_;
  }
}