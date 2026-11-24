using System;
using System.Collections;

namespace Proje1
{
    public class DiziListe : IEnumerable
    {
        private object[] _dizi;
        private int _elemanSayisi;

        public DiziListe(int baslangicKapasitesi = 4)
        {
            if (baslangicKapasitesi < 1)
                baslangicKapasitesi = 4;

            _dizi = new object[baslangicKapasitesi];
            _elemanSayisi = 0;
        }

        public int ElemanSayisi => _elemanSayisi;

        private void KapasiteKontrol()
        {
            if (_elemanSayisi >= _dizi.Length)
            {
                int yeniKapasite = _dizi.Length * 2;
                object[] yeniDizi = new object[yeniKapasite];
                Array.Copy(_dizi, yeniDizi, _elemanSayisi);
                _dizi = yeniDizi;
            }
        }

        public object this[int indeks]
        {
            get
            {
                if (indeks < 0 || indeks >= _elemanSayisi)
                    throw new ArgumentOutOfRangeException(nameof(indeks), "İndeks sınırların dışında.");

                return _dizi[indeks];
            }
            set
            {
                if (indeks < 0 || indeks >= _elemanSayisi)
                    throw new ArgumentOutOfRangeException(nameof(indeks), "İndeks sınırların dışında.");

                _dizi[indeks] = value;
            }
        }

        public void SonaEkle(object eleman)
        {
            KapasiteKontrol();
            _dizi[_elemanSayisi] = eleman;
            _elemanSayisi++;
        }

        public void ArayaEkle(int indeks, object eleman)
        {
            if (indeks < 0 || indeks > _elemanSayisi)
                throw new ArgumentOutOfRangeException(nameof(indeks), "İndeks sınırların dışında.");

            KapasiteKontrol();

            for (int i = _elemanSayisi; i > indeks; i--)
            {
                _dizi[i] = _dizi[i - 1];
            }

            _dizi[indeks] = eleman;
            _elemanSayisi++;
        }

        public bool Sil(object eleman)
        {
            int indeks = Bul(eleman);
            if (indeks == -1)
                return false;

            Sil(indeks);
            return true;
        }

        public void Sil(int indeks)
        {
            if (indeks < 0 || indeks >= _elemanSayisi)
                throw new ArgumentOutOfRangeException(nameof(indeks), "İndeks sınırların dışında.");

            for (int i = indeks; i < _elemanSayisi - 1; i++)
            {
                _dizi[i] = _dizi[i + 1];
            }

            _elemanSayisi--;
            _dizi[_elemanSayisi] = null!;
        }

        public bool VarMi(object eleman)
        {
            return Bul(eleman) != -1;
        }

        public int Bul(object eleman)
        {
            for (int i = 0; i < _elemanSayisi; i++)
            {
                if (_dizi[i] == null && eleman == null)
                    return i;

                if (_dizi[i] != null && _dizi[i].Equals(eleman))
                    return i;
            }

            return -1;
        }

        public void Temizle()
        {
            Array.Clear(_dizi, 0, _elemanSayisi);
            _elemanSayisi = 0;
        }

        public void Sirala()
        {
            if (_elemanSayisi <= 1)
                return;

            for (int i = 0; i < _elemanSayisi - 1; i++)
            {
                for (int j = 0; j < _elemanSayisi - 1 - i; j++)
                {
                    if (_dizi[j] is IComparable a && _dizi[j + 1] is IComparable b)
                    {
                        if (a.CompareTo(b) > 0)
                        {
                            object gecici = _dizi[j];
                            _dizi[j] = _dizi[j + 1];
                            _dizi[j + 1] = gecici;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Listedeki elemanlar karşılaştırılabilir (IComparable) değil.");
                    }
                }
            }
        }

        public void TerCevir()
        {
            int sol = 0;
            int sag = _elemanSayisi - 1;

            while (sol < sag)
            {
                object gecici = _dizi[sol];
                _dizi[sol] = _dizi[sag];
                _dizi[sag] = gecici;

                sol++;
                sag--;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _elemanSayisi; i++)
            {
                yield return _dizi[i];
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DiziListe liste = new DiziListe();

            liste.SonaEkle(30);
            liste.SonaEkle(10);
            liste.SonaEkle(50);
            liste.ArayaEkle(1, 20);

            Console.WriteLine("Eleman Sayısı: " + liste.ElemanSayisi);
            Console.WriteLine("İndeks 1: " + liste[1]);

            liste[1] = 25;
            Console.WriteLine("Güncellenen İndeks 1: " + liste[1]);

            Console.WriteLine("50 Var mı? " + liste.VarMi(50));
            Console.WriteLine("30'un İndeksi: " + liste.Bul(30));

            liste.Sil((object)25); // 25 değerini nesne olarak siler
            liste.Sil(0);          // 0. indeksteki elemanı siler

            liste.SonaEkle(5);
            liste.Sirala();
            Console.WriteLine("\nSıralı Liste:");
            foreach (var item in liste)
            {
                Console.Write(item + " ");
            }

            liste.TerCevir();
            Console.WriteLine("\nTers Çevrilmiş Liste:");
            foreach (var item in liste)
            {
                Console.Write(item + " ");
            }

            liste.Temizle();
            Console.WriteLine("\nTemizlendikten Sonra Eleman Sayısı: " + liste.ElemanSayisi);

            Console.ReadLine();
        }
    }
}