using System;
using System.Collections;

namespace Proje5
{
    public class SiraliListe : IEnumerable
    {
        private object[] _anahtarlar;
        private object[] _degerler;
        private int _elemanSayisi;

        public SiraliListe(int baslangicKapasitesi = 4)
        {
            if (baslangicKapasitesi < 1)
                baslangicKapasitesi = 4;

            _anahtarlar = new object[baslangicKapasitesi];
            _degerler = new object[baslangicKapasitesi];
            _elemanSayisi = 0;
        }

        public int ElemanSayisi => _elemanSayisi;

        public object[] Anahtarlar
        {
            get
            {
                object[] sonuc = new object[_elemanSayisi];
                Array.Copy(_anahtarlar, sonuc, _elemanSayisi);
                return sonuc;
            }
        }

        public object[] Degerler
        {
            get
            {
                object[] sonuc = new object[_elemanSayisi];
                Array.Copy(_degerler, sonuc, _elemanSayisi);
                return sonuc;
            }
        }

        public int HashUret(object anahtar)
        {
            if (anahtar == null)
                throw new ArgumentNullException(nameof(anahtar), "Anahtar boş olamaz.");

            return anahtar.GetHashCode() & 0x7FFFFFFF;
        }

        private void KapasiteKontrol()
        {
            if (_elemanSayisi >= _anahtarlar.Length)
            {
                int yeniKapasite = _anahtarlar.Length * 2;
                object[] yeniAnahtarlar = new object[yeniKapasite];
                object[] yeniDegerler = new object[yeniKapasite];

                Array.Copy(_anahtarlar, yeniAnahtarlar, _elemanSayisi);
                Array.Copy(_degerler, yeniDegerler, _elemanSayisi);

                _anahtarlar = yeniAnahtarlar;
                _degerler = yeniDegerler;
            }
        }

        private int AnahtarIndeksBul(object anahtar)
        {
            if (!(anahtar is IComparable aranan))
                throw new ArgumentException("Anahtar karşılaştırılabilir (IComparable) olmalıdır.");

            int sol = 0;
            int sag = _elemanSayisi - 1;

            while (sol <= sag)
            {
                int orta = sol + (sag - sol) / 2;
                int karsilastirma = aranan.CompareTo(_anahtarlar[orta]);

                if (karsilastirma == 0)
                    return orta;
                else if (karsilastirma < 0)
                    sag = orta - 1;
                else
                    sol = orta + 1;
            }

            return ~sol;
        }

        public void Ekle(object anahtar, object deger)
        {
            if (anahtar == null)
                throw new ArgumentNullException(nameof(anahtar), "Anahtar boş olamaz.");

            int indeks = AnahtarIndeksBul(anahtar);
            if (indeks >= 0)
                throw new ArgumentException("Aynı anahtara sahip bir eleman zaten mevcut.");

            int eklemeIndeksi = ~indeks;
            KapasiteKontrol();

            for (int i = _elemanSayisi; i > eklemeIndeksi; i--)
            {
                _anahtarlar[i] = _anahtarlar[i - 1];
                _degerler[i] = _degerler[i - 1];
            }

            _anahtarlar[eklemeIndeksi] = anahtar;
            _degerler[eklemeIndeksi] = deger;
            _elemanSayisi++;
        }

        public bool Sil(object anahtar)
        {
            if (anahtar == null)
                throw new ArgumentNullException(nameof(anahtar), "Anahtar boş olamaz.");

            int indeks = AnahtarIndeksBul(anahtar);
            if (indeks < 0)
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
                _anahtarlar[i] = _anahtarlar[i + 1];
                _degerler[i] = _degerler[i + 1];
            }

            _elemanSayisi--;
            _anahtarlar[_elemanSayisi] = null!;
            _degerler[_elemanSayisi] = null!;
        }

        public void Temizle()
        {
            Array.Clear(_anahtarlar, 0, _elemanSayisi);
            Array.Clear(_degerler, 0, _elemanSayisi);
            _elemanSayisi = 0;
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _elemanSayisi; i++)
            {
                yield return new DictionaryEntry(_anahtarlar[i], _degerler[i]);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SiraliListe liste = new SiraliListe();

            liste.Ekle(30, "Otuz");
            liste.Ekle(10, "On");
            liste.Ekle(20, "Yirmi");
            liste.Ekle(5, "Beş");

            Console.WriteLine("Eleman Sayısı: " + liste.ElemanSayisi);
            Console.WriteLine("20 Anahtarının Hash Kodu: " + liste.HashUret(20));

            Console.WriteLine("\nSıralı Kayıtlar (foreach):");
            foreach (DictionaryEntry entry in liste)
            {
                Console.WriteLine($"{entry.Key} -> {entry.Value}");
            }

            Console.WriteLine("\nAnahtarlar:");
            foreach (var k in liste.Anahtarlar)
            {
                Console.Write(k + " ");
            }

            Console.WriteLine("\n\nDeğerler:");
            foreach (var v in liste.Degerler)
            {
                Console.Write(v + " ");
            }

            liste.Sil((object)20);
            liste.Sil(0);

            Console.WriteLine("\n\nSilme İşlemleri Sonrası Kayıtlar:");
            foreach (DictionaryEntry entry in liste)
            {
                Console.WriteLine($"{entry.Key} -> {entry.Value}");
            }

            liste.Temizle();
            Console.WriteLine("\nTemizlendikten Sonra Eleman Sayısı: " + liste.ElemanSayisi);

            Console.ReadLine();
        }
    }
}