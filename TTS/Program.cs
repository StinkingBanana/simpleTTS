using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;

namespace TTS
{
    class Program
    {
        static void Main(string[] args)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            System.Globalization.CultureInfo zh = new System.Globalization.CultureInfo("zh-TW");
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo ja = new System.Globalization.CultureInfo("ja-JP");

            while (true)
            {
                string toRead = Console.ReadLine();
                toRead = Encoding.UTF8.GetString(Convert.FromBase64String(toRead));

                toRead = Regex.Replace(toRead,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "LINK");

                if(ContainsJapanese(toRead))
                    synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Child, 0, ja);
                else if (ContainsOnlyEnglishAndNumber(toRead))
                    synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Child, 0, en);
                else
                    synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Child, 0, zh);

                synth.Speak(toRead);
            }
  
        }

        static bool ContainsJapanese(string text)
        {
            if (text == null) return false;

            return ContainsCharacterInRange(text, 0x3040, 0x309F) || ContainsCharacterInRange(text, 0x30A0, 0x30FF);
        }

        static bool ContainsOnlyEnglishAndNumber(string text)
        {
            if (text == null) return false;

            return Regex.IsMatch(text, "^[a-zA-Z0-9 ]*$");
        }

        static bool ContainsCharacterInRange(string text, int min, int max)
        {
            return text.Any(e => e >= min && e <= max);
        }
    }
}
