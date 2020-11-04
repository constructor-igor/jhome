using System.Collections.Generic;
using System.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class GematriaCalculator
    {
        Dictionary<int, int> m_gematriaDictionary = new Dictionary<int, int>();
        public GematriaCalculator(){
            m_gematriaDictionary.Add(0x05D0, 1);      // Aleph
            m_gematriaDictionary.Add(0x05D1, 2);      // Bet
            m_gematriaDictionary.Add(0x05D2, 3);      // Gimel
            m_gematriaDictionary.Add(0x05D3, 4);      // Daleth
            m_gematriaDictionary.Add(0x05D4, 5);      // Heh
            m_gematriaDictionary.Add(0x05D5, 6);      // Vav
            m_gematriaDictionary.Add(0x05D6, 7);      // Zayin
            m_gematriaDictionary.Add(0x05D7, 8);      // Het
            m_gematriaDictionary.Add(0x05D8, 9);      // Tet
            m_gematriaDictionary.Add(0x05D9, 10);     // Yud
            m_gematriaDictionary.Add(0x05DA, 20);     // Kaf(final)   // 500
            m_gematriaDictionary.Add(0x05DB, 20);     // Kaf
            m_gematriaDictionary.Add(0x05DC, 30);     // Lamed
            m_gematriaDictionary.Add(0x05DD, 40);     // Mem(final)   // 600
            m_gematriaDictionary.Add(0x05DE, 40);     // Mem
            m_gematriaDictionary.Add(0x05DF, 50);     // Nun(final)   // 700
            m_gematriaDictionary.Add(0x05E0, 50);     // Nun
            m_gematriaDictionary.Add(0x05E1, 60);     // Samech
            m_gematriaDictionary.Add(0x05E2, 70);     // Ayin
            m_gematriaDictionary.Add(0x05E3, 80);     // Peh(final)   // 800-
            m_gematriaDictionary.Add(0x05E4, 80);     // Peh
            m_gematriaDictionary.Add(0x05E5, 90);     // Tzady(final) // 900
            m_gematriaDictionary.Add(0x05E6, 90);     // Tzady
            m_gematriaDictionary.Add(0x05E7, 100);    // Koof
            m_gematriaDictionary.Add(0x05E8, 200);    // Reish
            m_gematriaDictionary.Add(0x05E9, 300);    // Shin
            m_gematriaDictionary.Add(0x05EA, 400);    // Taf
        }
        public int Calculate(string text)
        {
            int resultValue = text.ToArray().Sum(v =>
            {
                int vr;
                if (m_gematriaDictionary.TryGetValue(v, out vr))
                {
                    return vr;
                }
                return 0;
            });
            return resultValue;
        }
    }
}