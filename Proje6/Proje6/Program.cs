using System;
using System.Collections;
using System.Collections.Generic;

namespace Proje6
{
    public class Liste<T> : IEnumerable<T>
    {
        private T[] _dizi;
        private int _elemanSayisi;

        public Liste(int baslangicKapasitesi = 4)
        {
            if (baslangicKapasitesi < 1)
                baslangicKapasitesi = 4;

            _dizi = new T[baslangicKapasitesi];
            _elemanSayisi = 0;
        }

        public int ElemanSayisi => _elemanSayisi;

        private void KapasiteKontrol()
        {
            if (_elemanSayisi >= _dizi.Length)
            {
                int yeniKapasite = _dizi.Length * 2;
                T[] yeniDizi = new T[yeniKapasite];
                Array.Copy(_dizi, yeniDizi, _elemanSayisi);
                _dizi = yeniDizi;
            }
        }

        public T this[int indeks]
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

        public void Ekle(T eleman)
        {
            KapasiteKontrol();
            _dizi[_elemanSayisi] = eleman;
            _elemanSayisi++;
        }

        public void Ekle(int indeks, T eleman)
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

        public bool Sil(T eleman)
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
            _dizi[_elemanSayisi] = default!;
        }

        public bool VarMi(T eleman)
        {
            return Bul(eleman) != -1;
        }

        public int Bul(T eleman)
        {
            EqualityComparer<T> karsilastirici = EqualityComparer<T>.Default;

            for (int i = 0; i < _elemanSayisi; i++)
            {
                if (karsilastirici.Equals(_dizi[i], eleman))
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

            Comparer<T> karsilastirici = Comparer<T>.Default;

            for (int i = 0; i < _elemanSayisi - 1; i++)
            {
                for (int j = 0; j < _elemanSayisi - 1 - i; j++)
                {
                    if (karsilastirici.Compare(_dizi[j], _dizi[j + 1]) > 0)
                    {
                        T gecici = _dizi[j];
                        _dizi[j] = _dizi[j + 1];
                        _dizi[j + 1] = gecici;
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
                T gecici = _dizi[sol];
                _dizi[sol] = _dizi[sag];
                _dizi[sag] = gecici;

                sol++;
                sag--;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _elemanSayisi; i++)
            {
                yield return _dizi[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Liste<string> sehirler = new Liste<string>();

            sehirler.Ekle("Iğdır");
            sehirler.Ekle("Fethiye");
            sehirler.Ekle("Kütahya");
            sehirler.Ekle(1, "İstanbul");

            Console.WriteLine("Eleman Sayısı: " + sehirler.ElemanSayisi);
            Console.WriteLine("İndeks 1: " + sehirler[1]);

            sehirler[1] = "İzmir";
            Console.WriteLine("Güncellenen İndeks 1: " + sehirler[1]);

            Console.WriteLine("Fethiye Var mı? " + sehirler.VarMi("Fethiye"));
            Console.WriteLine("Kütahya'nın İndeksi: " + sehirler.Bul("Kütahya"));

            sehirler.Sil("İzmir");
            sehirler.Sil(0);

            sehirler.Ekle("Ankara");
            sehirler.Sirala();
            Console.WriteLine("\nSıralı Liste:");
            foreach (var sehir in sehirler)
            {
                Console.Write(sehir + " ");
            }

            sehirler.TerCevir();
            Console.WriteLine("\nTers Çevrilmiş Liste:");
            foreach (var sehir in sehirler)
            {
                Console.Write(sehir + " ");
            }

            sehirler.Temizle();
            Console.WriteLine("\nTemizlendikten Sonra Eleman Sayısı: " + sehirler.ElemanSayisi);

            Console.ReadLine();
        }
    }
}