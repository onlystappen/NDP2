using System;
using System.Collections;

namespace Proje2
{
    public class Kuyruk : IEnumerable
    {
        private object[] _dizi;
        private int _basIndeks;
        private int _sonIndeks;
        private int _elemanSayisi;

        public Kuyruk(int baslangicKapasitesi = 4)
        {
            if (baslangicKapasitesi < 1)
                baslangicKapasitesi = 4;

            _dizi = new object[baslangicKapasitesi];
            _basIndeks = 0;
            _sonIndeks = 0;
            _elemanSayisi = 0;
        }

        // ElemanSayisi 
        public int ElemanSayisi => _elemanSayisi;

        private void KapasiteKontrol()
        {
            if (_elemanSayisi >= _dizi.Length)
            {
                int yeniKapasite = _dizi.Length * 2;
                object[] yeniDizi = new object[yeniKapasite];

                //Elemanları yeni diziye
                for (int i = 0; i < _elemanSayisi; i++)
                {
                    yeniDizi[i] = _dizi[(_basIndeks + i) % _dizi.Length];
                }

                _dizi = yeniDizi;
                _basIndeks = 0;
                _sonIndeks = _elemanSayisi;
            }
        }

        
        public void KuyrugaGir(object eleman)
        {
            KapasiteKontrol();
            _dizi[_sonIndeks] = eleman;
            _sonIndeks = (_sonIndeks + 1) % _dizi.Length;
            _elemanSayisi++;
        }

        
        public object KuyruktanCik()
        {
            if (_elemanSayisi == 0)
                throw new InvalidOperationException("Kuyruk boş, eleman çıkarılamaz.");

            object deger = _dizi[_basIndeks];
            _dizi[_basIndeks] = null!;
            _basIndeks = (_basIndeks + 1) % _dizi.Length;
            _elemanSayisi--;

            return deger;
        }

        
        public object Bak()
        {
            if (_elemanSayisi == 0)
                throw new InvalidOperationException("Kuyruk boş.");

            return _dizi[_basIndeks];
        }

        
        public void Temizle()
        {
            for (int i = 0; i < _elemanSayisi; i++)
            {
                _dizi[(_basIndeks + i) % _dizi.Length] = null!;
            }

            _basIndeks = 0;
            _sonIndeks = 0;
            _elemanSayisi = 0;
        }

        
        public object[] Diziye()
        {
            object[] sonuc = new object[_elemanSayisi];
            for (int i = 0; i < _elemanSayisi; i++)
            {
                sonuc[i] = _dizi[(_basIndeks + i) % _dizi.Length];
            }
            return sonuc;
        }

        
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _elemanSayisi; i++)
            {
                yield return _dizi[(_basIndeks + i) % _dizi.Length];
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Kuyruk kuyruk = new Kuyruk();

            kuyruk.KuyrugaGir("Birinci");
            kuyruk.KuyrugaGir("İkinci");
            kuyruk.KuyrugaGir("Üçüncü");
            kuyruk.KuyrugaGir(4);

            Console.WriteLine("Eleman Sayısı: " + kuyruk.ElemanSayisi);
            Console.WriteLine("Sıradaki (Bak): " + kuyruk.Bak());

            Console.WriteLine("\nKuyruk Elemanları (foreach):");
            foreach (var item in kuyruk)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\nÇıkarılan: " + kuyruk.KuyruktanCik());
            Console.WriteLine("Yeni Sıradaki (Bak): " + kuyruk.Bak());
            Console.WriteLine("Kalan Eleman Sayısı: " + kuyruk.ElemanSayisi);

            object[] dizi = kuyruk.Diziye();
            Console.WriteLine("\nDiziye Aktarılan Eleman Sayısı: " + dizi.Length);

            kuyruk.Temizle();
            Console.WriteLine("Temizlendikten Sonra Eleman Sayısı: " + kuyruk.ElemanSayisi);

            Console.ReadLine();
        }
    }
}