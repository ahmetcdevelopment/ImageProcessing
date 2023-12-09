using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoruntuIsleme.Enums
{
    public class IslemTypes
    {
        public enum BirinciIslemMenu
        {
            GriSeviye=0,
            SiyahBeyaz=1,
            ZoomIn=2,
            ZoomOut=3,
            KesipAl=4,
        }
        public enum IkinciIslemMenu
        {
            HistogramEsitle=0,
            Nicemleme=1,
        }
        public enum FiltreIslemMenu
        {
            Gauss=0,
            Keskinlestirme=1,
            KenarBulma=2,
            OrtalamaFiltresi=3,
            OrtancaFiltresi=4,
            KontraharmonikFiltre=5
        }
        public enum MorfolojikIslemMenu
        {
            Genisletme=0,
            Erozyon=1,
            IskeletCikart=2,
        }
    }
}
