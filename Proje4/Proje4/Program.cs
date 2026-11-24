using System;
using System.Collections;

namespace Proje4
{
    public class HashTablo : IEnumerable
    {
        private struct Dugum
        {
            public object Anahtar;
            public object Deger;
            public bool Dolu;
            public bool Silindi;
        }

        private Dugum[] _tablo;
        private int _elemanSayisi;

        public HashTablo(int baslangicKapasitesi = 16)
        {
            if (baslangicKapasitesi < 4)
                baslangicKapasitesi = 16;

            _tablo = new Dugum[baslangicKapasitesi];
            _elemanSayisi = 0;
        }

        public int ElemanSayisi => _elemanSayisi;

        public object[] Anahtarlar
        {
            get
            {
                object[] sonuc = new object[_elemanSayisi];
                int sayac = 0;

                for (int i = 0; i < _tablo.Length; i++)
                {
                    if (_tablo[i].Dolu && !_tablo[i].Silindi)
                    {
                        sonuc[sayac] = _tablo[i].Anahtar;
                        sayac++;
                    }
                }

                return sonuc;
            }
        }

        public object[] Degerler
        {
            get
            {
                object[] sonuc = new object[_elemanSayisi];
                int sayac = 0;

                for (int i = 0; i < _tablo.Length; i++)
                {
                    if (_tablo[i].Dolu && !_tablo[i].Silindi)
                    {
                        sonuc[sayac] = _tablo[i].Deger;
                        sayac++;
                    }
                }

                return sonuc;
            }
        }

        public int HashUret(object anahtar)
        {
            if (anahtar == null)
                throw new ArgumentNullException(nameof(anahtar), "Anahtar boş olamaz.");

            int hamHash = anahtar.GetHashCode() & 0x7FFFFFFF;
            return hamHash % _tablo.Length;
        }

        private void KapasiteKontrol()
        {
            if ((double)(_elemanSayisi + 1) / _tablo.Length >= 0.75)
            {
                Dugum[] eskiTablo = _tablo;
                _tablo = new Dugum[eskiTablo.Length * 2];
                _elemanSayisi = 0;

                for (int i = 0; i < eskiTablo.Length; i++)
                {
                    if (eskiTablo[i].Dolu && !eskiTablo[i].Silindi)
                    {
                        Ekle(eskiTablo[i].Anahtar, eskiTablo[i].Deger);
                    }
                }
            }
        }

        public void Ekle(object anahtar, object deger)
        {
            if (anahtar == null)
                throw new ArgumentNullException(nameof(anahtar), "Anahtar boş olamaz.");

            KapasiteKontrol();

            int indeks = HashUret(anahtar);
            int ilkSilinenIndeks = -1;

            for (int i = 0; i < _tablo.Length; i++)
            {
                int mevcutIndeks = (indeks + i) % _tablo.Length;

                if (!_tablo[mevcutIndeks].Dolu)
                {
                    int hedefIndeks = ilkSilinenIndeks != -1 ? ilkSilinenIndeks : mevcutIndeks;
                    _tablo[hedefIndeks] = new Dugum
                    {
                        Anahtar = anahtar,
                        Deger = deger,
                        Dolu = true,
                        Silindi = false
                    };
                    _elemanSayisi++;
                    return;
                }

                if (_tablo[mevcutIndeks].Silindi && ilkSilinenIndeks == -1)
                {
                    ilkSilinenIndeks = mevcutIndeks;
                }
                else if (!_tablo[mevcutIndeks].Silindi && _tablo[mevcutIndeks].Anahtar.Equals(anahtar))
                {
                    throw new ArgumentException("Aynı anahtara sahip bir eleman zaten mevcut.");
                }
            }

            if (ilkSilinenIndeks != -1)
            {
                _tablo[ilkSilinenIndeks] = new Dugum
                {
                    Anahtar = anahtar,
                    Deger = deger,
                    Dolu = true,
                    Silindi = false
                };
                _elemanSayisi++;
            }
        }

        public bool Sil(object anahtar)
        {
            if (anahtar == null)
                throw new ArgumentNullException(nameof(anahtar), "Anahtar boş olamaz.");

            int indeks = HashUret(anahtar);

            for (int i = 0; i < _tablo.Length; i++)
            {
                int mevcutIndeks = (indeks + i) % _tablo.Length;

                if (!_tablo[mevcutIndeks].Dolu && !_tablo[mevcutIndeks].Silindi)
                    return false;

                if (_tablo[mevcutIndeks].Dolu && !_tablo[mevcutIndeks].Silindi && _tablo[mevcutIndeks].Anahtar.Equals(anahtar))
                {
                    _tablo[mevcutIndeks].Silindi = true;
                    _tablo[mevcutIndeks].Anahtar = null!;
                    _tablo[mevcutIndeks].Deger = null!;
                    _elemanSayisi--;
                    return true;
                }
            }

            return false;
        }

        public void Temizle()
        {
            for (int i = 0; i < _tablo.Length; i++)
            {
                _tablo[i] = default;
            }
            _elemanSayisi = 0;
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _tablo.Length; i++)
            {
                if (_tablo[i].Dolu && !_tablo[i].Silindi)
                {
                    yield return new DictionaryEntry(_tablo[i].Anahtar, _tablo[i].Deger);
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            HashTablo tablo = new HashTablo();

            tablo.Ekle("ad", "Arda");
            tablo.Ekle("sehir", "Fethiye");
            tablo.Ekle(1, "Birinci");

            Console.WriteLine("Eleman Sayısı: " + tablo.ElemanSayisi);
            Console.WriteLine("'sehir' Hash Kodu (İndeks): " + tablo.HashUret("sehir"));

            Console.WriteLine("\nAnahtarlar:");
            foreach (var k in tablo.Anahtarlar)
            {
                Console.WriteLine(k);
            }

            Console.WriteLine("\nDeğerler:");
            foreach (var v in tablo.Degerler)
            {
                Console.WriteLine(v);
            }

            Console.WriteLine("\nTüm Kayıtlar (foreach):");
            foreach (DictionaryEntry entry in tablo)
            {
                Console.WriteLine($"{entry.Key} -> {entry.Value}");
            }

            tablo.Sil(1);
            Console.WriteLine("\n'1' Silindikten Sonra Eleman Sayısı: " + tablo.ElemanSayisi);

            tablo.Temizle();
            Console.WriteLine("Temizlendikten Sonra Eleman Sayısı: " + tablo.ElemanSayisi);

            Console.ReadLine();
        }
    }
}