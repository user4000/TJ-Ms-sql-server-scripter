using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ProjectStandard
{
  public class TestTTRat
  {
    private RadTextBox TxtMessage { get; } = null;
    private RadTextBoxControl TxtTest { get; } = null;
    private RadButton BtnTest1 { get; } = null;
    private RadForm TestForm { get; } = null;



    public TestTTRat(RadForm form, RadButton button, RadTextBoxControl input, RadTextBox output)
    {
      TestForm = form; BtnTest1 = button; TxtTest = input; TxtMessage = output;
    }

    private string LineSeparator { get; } = Environment.NewLine + Environment.NewLine;

    private void Print(string s) => TxtMessage.AppendText(s + LineSeparator);

    private void Print(byte[] array) => Print(TTRat.Debug(array));

    public void PrintTestResult(string TestName, byte[] AA, byte[] BB)
    {
      if (TTRat.ByteArrayCompare(AA, BB))
      { Print($"Test [{TestName}] is OK "); }
      else
      { Print($"Error! Test [{TestName}] failed"); };
    }

    public void PrintTestResult(string TestName, string AA, string BB)
    {
      if (AA == BB)
      { Print($"Test [{TestName}] is OK "); }
      else
      { Print($"Error! Test [{TestName}] failed"); };
    }

    public void TestAddRandomArrays()
    {
      TxtTest.Text = TxtTest.Text.Trim();

      if (TxtTest.Text.Length < 1) { TxtTest.Text = "Privet 12345 !"; }

      string s = TxtTest.Text;

      byte[] AA = Encoding.UTF8.GetBytes(s);
      Print(AA);

      byte[] BB = TTRat.AddRandomArraysFromRightAndFromLeft(AA, 151, 999, 151, 999);
      Print(BB);

      s = Convert.ToBase64String(BB);
      Print(s);

      byte[] DD = Convert.FromBase64String(s);

      byte[] CC = TTRat.RemoveRandomArraysFromRightAndFromLeft(DD);
      Print(CC);

      s = Encoding.UTF8.GetString(CC);

      Print(s);

      PrintTestResult("Random Arrays", AA, CC);
    }

    public void TestSBox()
    {
      int TestRounds = 10000;
      int ArrayLength = 1000;

      byte[] AA = TTRat.RandomArray(ArrayLength);
      byte[] BB = TTRat.RandomArray(ArrayLength);
      byte[] CC = TTRat.RandomArray(ArrayLength);


      for (int i = 0; i < ArrayLength; i++) AA[i] = 0;
      Print(AA);

      BB = TTRat.RijndaelSBox(AA);
      Print(BB);

      for (int i = 0; i < ArrayLength; i++) AA[i] = 255;
      Print(AA);

      BB = TTRat.RijndaelSBox(AA);
      Print(BB);

      BtnTest1.Enabled = false;
      Application.DoEvents();
    
      for (int i = 0; i < TestRounds; i++)
      {
        AA = TTRat.RandomArray(ArrayLength);

        //for (int j = 0; j < AA.Length; j++) BB[j] = AA[j];

        BB = TTRat.RijndaelSBox(AA);
        CC = TTRat.RijndaelInvSBox(BB);

        if (TTRat.ByteArrayCompare(CC, AA) == false)
        {
          Print($"Error! step {i}");
          Print(AA);
          Print(CC);
          break;
        }

        if (i % 100 == 0) Application.DoEvents();

      }

      BtnTest1.Enabled = true;

      Print($"Test SBOX passed. Rounds = {TestRounds}");
    }

    public void TestTJSerpent()
    {
      int TestRounds = 10000;
      int ArrayLength = 1000;

      byte[] AA = TTRat.RandomArray(ArrayLength);
      byte[] BB = TTRat.RandomArray(ArrayLength);

      for (int i = 0; i < ArrayLength; i++) AA[i] = 0;
      Print(AA);

      TTRat.TjSerpentF(AA);
      Print(AA);

      for (int i = 0; i < ArrayLength; i++) AA[i] = 255;
      Print(AA);

      TTRat.TjSerpentF(AA);
      Print(AA);

      BtnTest1.Enabled = false;
      Application.DoEvents();

      TestForm.Refresh();

      for (int i = 0; i < TestRounds; i++)
      {
        AA = TTRat.RandomArray(ArrayLength);

        for (int j = 0; j < AA.Length; j++) BB[j] = AA[j];

        TTRat.TjSerpentF(AA);
        TTRat.TjSerpentB(AA);

        if (TTRat.ByteArrayCompare(BB, AA) == false)
        {
          Print($"Error! step {i}");
          Print(AA);
          Print(BB);
          break;
        }

        if (i % 100 == 0) Application.DoEvents();

      }

      BtnTest1.Enabled = true;

      Print($"Test passed. Rounds = {TestRounds}");
    }

    public void TestHadamardTransformation()
    {
      int TestRounds1 = 1000000;

      int TestRounds2 = 1000;

      byte[] AA = TTRat.RandomArray(8);
      byte[] BB = TTRat.RandomArray(8);

      for (int i = 0; i < 8; i++) AA[i] = 0;
      Print(AA);

      TTRat.HTF(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);
      Print(AA);

      for (int i = 0; i < 8; i++) AA[i] = 255;
      Print(AA);

      TTRat.HTF(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);
      Print(AA);


      BtnTest1.Enabled = false;
      Application.DoEvents();

      TestForm.Refresh();


      Print($"Loop 1.");

      for (int i = 0; i < TestRounds1; i++)
      {
        AA = TTRat.RandomArray(8);

        for (int j = 0; j < AA.Length; j++) BB[j] = AA[j];

        TTRat.HTF(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);
        TTRat.HTB(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);

        if (TTRat.ByteArrayCompare(BB, AA) == false)
        {
          Print($"Loop 1. Error! step {i}");
          Print(AA);
          Print(BB);
          break;
        }

        if (i % 10000 == 0) Application.DoEvents();

      }
      Print($"Test passed. Rounds = {TestRounds1}");


      Print($"Loop 2.");

      int L = 0;

      for (int i = 0; i < TestRounds2; i++)
      {

        L = TTSecurityStandard.GetRandom(99, 1999);

        AA = TTRat.RandomArray(L);
        BB = new byte[L];
        TTRat.CopyArray(AA, BB);

        TTRat.HTF(AA);
        TTRat.HTB(AA);

        if (TTRat.ByteArrayCompare(BB, AA) == false)
        {
          Print($"Loop 2. Error! step {i}");
          Print(AA);
          Print(BB);
          break;
        }
      }

      Print($"Test passed. Rounds = {TestRounds2}");

      BtnTest1.Enabled = true;
    }

    public void TestByteArray()
    {
      string s = TxtTest.Text;
      byte[] array = Encoding.UTF8.GetBytes(s);

      byte[] rnd = TTRat.RandomArray(8);
      Print(rnd);

      for (int i = 1; i < 2; i++)
      {
        rnd = TTRat.RandomArray(8);
        TTRat.HTF(ref rnd[0], ref rnd[1], ref rnd[2], ref rnd[3], ref rnd[4], ref rnd[5], ref rnd[6], ref rnd[7]);
        //ms.Debug(i.ToString());
        Print(rnd);
      }
    }

    public void TestRadTextBoxControl()
    {
      TxtMessage.Clear();

      Stopwatch sw = Stopwatch.StartNew();

      Print($"Start.");

      byte[] AA = null; // TTRat.RandomArray(TTSecurityStandard.GetRandom(290, 300));

      for (int i = 0; i < 101; i++)
      {
        //Print($"This is a round number {i}");
        AA = TTRat.RandomArray(TTSecurityStandard.GetRandom(290, 300));
        //Print(AA);
      }

      sw.Stop();

      Print($"End. Milliseconds = {sw.ElapsedMilliseconds}");
    }

    public void TestAES()
    {
      TxtTest.Text = TxtTest.Text.Trim();
      if (TxtTest.Text.Length < 1) { TxtTest.Text = "Privet 12345 !"; }
      string s = TxtTest.Text;

      Stopwatch sw = Stopwatch.StartNew(); Print($"Start.");

      byte[] ArrayPlainText = TTRat.RandomArray(10000); //TTSecurityStandard.GetBytes(s);
      //Print(ArrayPlainText);

      byte[] ArrayPassword = TTSecurityStandard.GetBytes("My-Sect3t-P@55w0rD12345!");

      byte[] ArraySalt = TTSecurityStandard.GetBytes("This is a salt !!!");

      byte[] ArrayEncoded = TTSecurityStandard.Encrypt(ArrayPassword, new ArraySegment<byte>(ArrayPlainText), new ArraySegment<byte>(ArraySalt));

      //Print(ArrayEncoded);

      byte[] ArrayDecoded = TTSecurityStandard.Decrypt(ArrayPassword, new ArraySegment<byte>(ArrayEncoded), new ArraySegment<byte>(ArraySalt));

      PrintTestResult("AES", ArrayPlainText, ArrayDecoded);

      sw.Stop(); Print($"End. Array Length = {ArrayPlainText.Length} Milliseconds = {sw.ElapsedMilliseconds}");

      //s = TTSecurityStandard.GetString(ArrayDecoded);
      //Print(s);

    }

    public void TestCodeShift()
    {
      byte[] AA = TTRat.RandomArray( TTSecurityStandard.GetRandom(5,133) );
      byte[] BB = TTRat.CopyArray(AA);
      byte[] CC = Convert.FromBase64String(TTConstArray128.Array[TTSecurityStandard.GetRandom(0,4)]);

      TxtMessage.Clear();

      Print(AA);

      TTRat.CodeShiftF(AA, CC);

      Print(AA);

      TTRat.CodeShiftB(AA, CC);

      Print(AA);
      PrintTestResult("Code Shift", AA, BB);
    }

    public void TestXOR()
    {
      byte[] AA = TTRat.RandomArray( TTSecurityStandard.GetRandom(5,293) );
      byte[] BB = TTRat.CopyArray(AA);
      byte[] CC = Convert.FromBase64String(TTConstArray128.Array[TTSecurityStandard.GetRandom(0,4)]);

      TxtMessage.Clear();

      Print(AA);

      TTRat.XOR(AA, CC);

      Print(AA);

      TTRat.XOR(AA, CC);

      Print(AA);

      PrintTestResult("XOR", AA, BB);
    }

    public void TestArrayShift()
    {
      byte[] AA = TTRat.RandomArray( TTSecurityStandard.GetRandom(50,50) );

      for (int i = 0; i < AA.Length; i++) AA[i] = (byte)(i + 1);

      byte[] BB = TTRat.CopyArray(AA);
      byte[] CC = Convert.FromBase64String(TTConstArray128.Array[TTSecurityStandard.GetRandom(0,4)]);

      TxtMessage.Clear();
      Print(AA);

      TTRat.ShiftF(AA,17);    
      Print(AA);

      TTRat.ShiftB(AA,17);    
      Print(AA);
      PrintTestResult("ArrayShift", AA, BB);
    }

    public void TestAbstract1()
    {
      byte[] AA = TTRat.RandomArray(128);

      Print(AA);

      Print(Convert.ToBase64String(AA));
    }

    public void TestHadamardTransformationStatistic()
    {
      int Round = 10000;
      int DX = 0;

      int MinDX = 0;
      int MinIndex = 0;

      int MaxDX = 0;
      int MaxIndex = 0;

      byte[] AA = TTRat.RandomArray( TTSecurityStandard.GetRandom(1750,1750) );

      //for (int i = 0; i < AA.Length; i++) AA[i] = (byte)(i + 1);

      byte[] BB = TTRat.CopyArray(AA);
      byte[] CC = Convert.FromBase64String(TTConstArray128.Array[TTSecurityStandard.GetRandom(0,4)]);
      
      TxtMessage.Clear();
      Print(AA);

      for (int i = 0; i < Round; i++)
      {
        TTRat.HTF(AA);
        DX = TTRat.ByteArrayDX(BB, AA);
        //Print($"{i+1};{DX};");
        if (MinDX == 0) MinDX = DX;
        if (MinDX > DX)
        {
          MinDX = DX; MinIndex = i+1;
        }

        if (MaxDX < DX)
        {
          MaxDX = DX; MaxIndex = i+1;
        }
      }
           
      Print($"Minimum = {MinIndex};{MinDX}");

      Print($"Maximum = {MaxIndex};{MaxDX}");

      //Print(AA);

      for (int i = 0; i < Round; i++)
      {
        TTRat.HTB(AA);  
      }
          
      //Print(AA);
      PrintTestResult("Hadamard Transformation", AA, BB);

    }

    public void TestTJTransformation()
    {
      int TestRounds1 = 1000000;

      int TestRounds2 = 1000;

      byte[] AA = TTRat.RandomArray(8);
      byte[] BB = TTRat.RandomArray(8);

      for (int i = 0; i < 8; i++) AA[i] = 0;
      Print(AA);

      TTRat.Tj3F(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);
      Print(AA);

      for (int i = 0; i < 8; i++) AA[i] = 255;
      Print(AA);

      TTRat.Tj3B(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);
      Print(AA);


      BtnTest1.Enabled = false;
      Application.DoEvents();

      TestForm.Refresh();

      Print($"Loop 1.");

      for (int i = 0; i < TestRounds1; i++)
      {
        AA = TTRat.RandomArray(8);

        for (int j = 0; j < AA.Length; j++) BB[j] = AA[j];

        TTRat.Tj3F(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);
        TTRat.Tj3B(ref AA[0], ref AA[1], ref AA[2], ref AA[3], ref AA[4], ref AA[5], ref AA[6], ref AA[7]);

        if (TTRat.ByteArrayCompare(BB, AA) == false)
        {
          Print($"Loop 1. Error! step {i}");
          Print(AA);
          Print(BB);
          break;
        }

        if (i % 10000 == 0) Application.DoEvents();

      }
      Print($"Test passed. Rounds = {TestRounds1}");


      Print($"Loop 2.");

      int L = 0;

      for (int i = 0; i < TestRounds2; i++)
      {

        L = TTSecurityStandard.GetRandom(99, 1999);

        AA = TTRat.RandomArray(L);
        BB = new byte[L];
        TTRat.CopyArray(AA, BB);

        TTRat.Tj3F(AA);
        TTRat.Tj3B(AA);

        if (TTRat.ByteArrayCompare(BB, AA) == false)
        {
          Print($"Loop 2. Error! step {i}");
          Print(AA);
          Print(BB);
          break;
        }
      }

      Print($"Test passed. Rounds = {TestRounds2}");

      BtnTest1.Enabled = true;
    }

    public void TestTJTransformationStatistic()
    {
      int Round = 10000;
      int DX = 0;

      int MinDX = 0;
      int MinIndex = 0;

      int MaxDX = 0;
      int MaxIndex = 0;


      byte[] AA = TTRat.RandomArray( TTSecurityStandard.GetRandom(1750,1750) );

      //for (int i = 0; i < AA.Length; i++) AA[i] = (byte)(i + 1);

      byte[] BB = TTRat.CopyArray(AA);
      byte[] CC = Convert.FromBase64String(TTConstArray128.Array[TTSecurityStandard.GetRandom(0,4)]);
      
      TxtMessage.Clear();
      Print(AA);

      for (int i = 0; i < Round; i++)
      {
        TTRat.Tj3F(AA);
        DX = TTRat.ByteArrayDX(BB, AA);
        //Print($"{i+1};{DX};");
        if (MinDX == 0) MinDX = DX;
        if (MinDX > DX)
        {
          MinDX = DX; MinIndex = i+1;
        }

        if (MaxDX < DX)
        {
          MaxDX = DX; MaxIndex = i+1;
        }
      }
           
      Print($"Minimum = {MinIndex};{MinDX}");

      Print($"Maximum = {MaxIndex};{MaxDX}");

      //Print(AA);

      for (int i = 0; i < Round; i++)
      {
        TTRat.Tj3B(AA);  
      }
          
      //Print(AA);
      PrintTestResult("TJ Transformation", AA, BB);

    }

    public void TestStringTransformation()
    {
      TxtTest.Text = TTSecurityStandard.GenerateRandomString(TTSecurityStandard.GetRandom(5, 50));
      TxtTest.Text = TxtTest.Text.Trim();
      if (TxtTest.Text.Length < 1) { TxtTest.Text = TTSecurityStandard.GenerateRandomString( TTSecurityStandard.GetRandom(5,50) )  ; }
      string s = TxtTest.Text; TxtMessage.Clear();
      Print(s);

      string Encoded = TTRat.TransformF(s);
      Print(Encoded);
      string Decoded = TTRat.TransformB(Encoded);
      Print(Decoded);
      PrintTestResult("String Transformation", s, Decoded);
      
      /*
      Print($"TTConstArray128.ListArray.Count={TTConstArray128.ListArray.Count}");
  
      for (int i = 0; i < TTConstArray128.Array.Length; i++)
      {
        Print($" i = {i}");
        Print(TTConstArray128.ListArray[i]);
      }
      */
    }

    public void TestStringTransformationCycle()
    {
      int Rounds = 1000000;

      Print($"Test String Transformation Cycle {Rounds}");
      int MaximumTimeOfWorkHours = 12;
      long MaximumTimeMs = MaximumTimeOfWorkHours * 60 * 60 * 1000;

      string password = string.Empty;
      string Encoded = string.Empty;
      string Decoded = string.Empty;

      Stopwatch sw = Stopwatch.StartNew();

      for(int i=0; i<Rounds; i++)
      {
        password = TTSecurityStandard.GenerateRandomString(TTSecurityStandard.GetRandom(5, 50)); 
        Encoded = TTRat.TransformF(password);
        Application.DoEvents();
        Decoded = TTRat.TransformB(Encoded);

        if (password!=Decoded)
        {
          Print($" i = {i}");
          Print($"ERROR ! password={password} , Encoded={Encoded} , Decoded={Decoded}");
          break;
        }

        if (i % 100 == 0)
        {
          Print($" i = {i}");
          Application.DoEvents();

          if (sw.ElapsedMilliseconds > MaximumTimeMs)
          {
            Print($" Wow! I have worked {MaximumTimeOfWorkHours} HOURS (Rounds={i}) ! It's time to stop working! ");
            break;
          }
        }
      }

      sw.Stop();

      Print($" Done. Test finished in {sw.ElapsedMilliseconds / 1000} seconds = {sw.ElapsedMilliseconds / (1000*60*60)} HOURS ");

    }

  }
}



