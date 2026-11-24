using System;
using System.Collections;

namespace Proje3
{
    public class Yigin : IEnumerable
    {
        private object[] _dizi;
        private int _elemanSayisi;

        public Yigin(int baslangicKapasitesi = 4)
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

        public void Doldur(object eleman)
        {
            KapasiteKontrol();
            _dizi[_elemanSayisi] = eleman;
            _elemanSayisi++;
        }

        public object Cek()
        {
            if (_elemanSayisi == 0)
                throw new InvalidOperationException("Yığın boş olduğu için eleman çıkarma işlemi gerçekleştirilemez.");

            _elemanSayisi--;
            object deger = _dizi[_elemanSayisi];
            _dizi[_elemanSayisi] = null!;

            return deger;
        }

        public object Bak()
        {
            if (_elemanSayisi == 0)
                throw new InvalidOperationException("Yığın boş durumda.");

            return _dizi[_elemanSayisi - 1];
        }

        public void Temizle()
        {
            Array.Clear(_dizi, 0, _elemanSayisi);
            _elemanSayisi = 0;
        }

        public object[] Diziye()
        {
            object[] sonuc = new object[_elemanSayisi];
            for (int i = 0; i < _elemanSayisi; i++)
            {
                sonuc[i] = _dizi[_elemanSayisi - 1 - i];
            }
            return sonuc;
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = _elemanSayisi - 1; i >= 0; i--)
            {
                yield return _dizi[i];
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Yigin yigin = new Yigin();

            yigin.Doldur("İlk Giren");
            yigin.Doldur("İkinci Giren");
            yigin.Doldur("Üçüncü Giren");
            yigin.Doldur(40);

            Console.WriteLine("Eleman Sayısı: " + yigin.ElemanSayisi);
            Console.WriteLine("En Üstteki Eleman (Bak): " + yigin.Bak());

            Console.WriteLine("\nYığın Elemanları (LIFO - foreach):");
            foreach (var item in yigin)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\nÇekilen Eleman: " + yigin.Cek());
            Console.WriteLine("Yeni En Üstteki Eleman (Bak): " + yigin.Bak());
            Console.WriteLine("Kalan Eleman Sayısı: " + yigin.ElemanSayisi);

            object[] dizi = yigin.Diziye();
            Console.WriteLine("\nDiziye Aktarılan Eleman Sayısı: " + dizi.Length);

            yigin.Temizle();
            Console.WriteLine("Temizleme Sonrası Eleman Sayısı: " + yigin.ElemanSayisi);

            Console.ReadLine();
        }
    }
}