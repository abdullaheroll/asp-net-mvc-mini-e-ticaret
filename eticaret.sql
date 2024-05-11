-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Anamakine: 127.0.0.1
-- Üretim Zamanı: 09 Nis 2024, 23:39:57
-- Sunucu sürümü: 10.4.27-MariaDB
-- PHP Sürümü: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Veritabanı: `eticaret`
--

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `hakkimizdat`
--

CREATE TABLE `hakkimizdat` (
  `HakkimizdaID` int(11) NOT NULL,
  `Baslik` varchar(250) NOT NULL,
  `Icerik` varchar(1000) NOT NULL,
  `GorselURL` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `hakkimizdat`
--

INSERT INTO `hakkimizdat` (`HakkimizdaID`, `Baslik`, `Icerik`, `GorselURL`) VALUES
(1, 'Biz kimiz', '**BizimPlak**, 2024 yılında kurulmuş ve müşterilerine en iyi online alışveriş deneyimini sunmayı hedefleyen bir e-ticaret platformudur. Kaliteli ürünleri uygun fiyatlarla sunarak, alışverişinizi keyifli ve kolay hale getirmek için çalışıyoruz. Yerel üreticilerden dünya markalarına kadar geniş bir yelpazede ürün seçeneği ile hizmetinizdeyiz. Müşteri memnuniyetini öncelikli tutarak, güvenli ödeme seçenekleri ve hızlı kargo hizmeti ile siz değerli müşterilerimizin beğenisini kazanmayı amaçlıyoruz. Sürdürülebilirlik ve çevre dostu yaklaşımlarımızla, gelecek nesillere daha yaşanabilir bir dünya bırakmak için çaba gösteriyoruz. **BizimPlak** ailesi olarak, alışverişinizi bir sonraki seviyeye taşımak için buradayız!', '/Gorsel/65f98fe58c3ea_1710854117.png');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `iletisimt`
--

CREATE TABLE `iletisimt` (
  `MesajID` int(11) NOT NULL,
  `Isim` varchar(100) NOT NULL,
  `Eposta` varchar(100) NOT NULL,
  `MesajIcerik` varchar(1000) NOT NULL,
  `OlusturmaTarihi` date NOT NULL,
  `IpAdresi` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `iletisimt`
--

INSERT INTO `iletisimt` (`MesajID`, `Isim`, `Eposta`, `MesajIcerik`, `OlusturmaTarihi`, `IpAdresi`) VALUES
(7, 'Büşra', 'busra@mail.com', 'Merhaba ürününüzü beğendim ama kargo çok geç geldi.', '2024-03-22', '::1');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `kategorilert`
--

CREATE TABLE `kategorilert` (
  `KategoriID` int(11) NOT NULL,
  `KategoriAdi` varchar(500) NOT NULL,
  `KategoriAciklamasi` varchar(1000) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `kategorilert`
--

INSERT INTO `kategorilert` (`KategoriID`, `KategoriAdi`, `KategoriAciklamasi`) VALUES
(11, 'Halk Müziği', 'Halk müziği ile alakalı plaklari'),
(12, 'Arabesk Müzik', 'Arabesk müzik ile alakalı plaklar'),
(14, 'Rock', 'Rock müzik ile alakalı plaklar');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `kullanicilart`
--

CREATE TABLE `kullanicilart` (
  `KullaniciID` int(11) NOT NULL,
  `Isim` varchar(100) NOT NULL,
  `Eposta` varchar(100) NOT NULL,
  `Parola` varchar(65) NOT NULL,
  `Aktiflik` tinyint(1) NOT NULL,
  `KayitTarihi` date NOT NULL,
  `RolID` tinyint(4) NOT NULL,
  `TeslimatAdresi` varchar(500) DEFAULT NULL,
  `FaturaAdresi` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `kullanicilart`
--

INSERT INTO `kullanicilart` (`KullaniciID`, `Isim`, `Eposta`, `Parola`, `Aktiflik`, `KayitTarihi`, `RolID`, `TeslimatAdresi`, `FaturaAdresi`) VALUES
(1, 'Abdulah EROL', 'info@abdullaherol.com', 'apo', 1, '2024-02-01', 1, 'Dunaysır Mahallesi Mardin', 'Dunaysır Mahallesi Mardin'),
(26, 'admin', 'admin', '1234', 1, '2024-03-04', 3, '', ''),
(128, 'Fatmagül', 'fatmagul@mail.com', 'fatma', 1, '2024-03-18', 2, 'fatma ocağı caddesi bilmemney sokak no:55', 'fatma ocağı caddesi bilmemney sokak no:55');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `siparisdetayt`
--

CREATE TABLE `siparisdetayt` (
  `DetayID` int(11) NOT NULL,
  `SiparisID` int(11) NOT NULL,
  `SatisFiyati` decimal(6,2) NOT NULL,
  `FaturaAdresi` varchar(500) NOT NULL,
  `TeslimatAdresi` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `siparisdetayt`
--

INSERT INTO `siparisdetayt` (`DetayID`, `SiparisID`, `SatisFiyati`, `FaturaAdresi`, `TeslimatAdresi`) VALUES
(14, 15, '390.00', 'fatma ocağı caddesi bilmemney sokak no:55', 'fatma ocağı caddesi bilmemney sokak no:55'),
(15, 16, '340.00', 'fatma ocağı caddesi bilmemney sokak no:55', 'fatma ocağı caddesi bilmemney sokak no:55');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `siparislert`
--

CREATE TABLE `siparislert` (
  `SiparisID` int(11) NOT NULL,
  `UrunID` int(11) NOT NULL,
  `KullaniciID` int(11) NOT NULL,
  `OlusturmaTarihi` date NOT NULL,
  `Durum` smallint(6) NOT NULL,
  `FaturaURL` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `siparislert`
--

INSERT INTO `siparislert` (`SiparisID`, `UrunID`, `KullaniciID`, `OlusturmaTarihi`, `Durum`, `FaturaURL`) VALUES
(15, 39, 128, '2024-03-28', 4, '/Fatura/6604b3855f054_1711584133.pdf'),
(16, 21, 128, '2024-03-28', 2, NULL);

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `urunlert`
--

CREATE TABLE `urunlert` (
  `UrunID` int(11) NOT NULL,
  `UrunAdi` varchar(500) NOT NULL,
  `UrunAciklamasi` text NOT NULL,
  `Fiyat` decimal(6,2) NOT NULL,
  `StokAdet` smallint(6) NOT NULL,
  `KategoriID` smallint(6) NOT NULL,
  `UrunGorselURL` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `urunlert`
--

INSERT INTO `urunlert` (`UrunID`, `UrunAdi`, `UrunAciklamasi`, `Fiyat`, `StokAdet`, `KategoriID`, `UrunGorselURL`) VALUES
(17, 'Müslüm Gürses - Paramparça (PLAK)', 'A 1 - Paramparça A 2 - Yitik Aşk A 3 - Aşk Bir Ateş A 4 - Sonuna Kadar A 5 - Çay Karam B 1 - Kara Gözler B 2 - Ecele Sitem B 3 - Sen Yoksun B 4 - Olan Bana Oldu B 5 - Seko Seko B 6 - Yitik Aşk (Enstrümantal)', '399.00', 52, 11, '/Gorsel/65f9845e62ea4_1710851166.png'),
(18, 'Aşık Mahzuni Şerif - Dumanlı Dumanlı Oy Bizim Eller', 'A Yüzü: Dumanlı Dumanlı Oy Bizim Eller Bitmez Tükenmez Geceler İhtiyar Oldum Sen Açtın Yarayı Zam Üstüne Zam Ermedim Murada B Yüzü: İnce İnce Bir Kar Yağar Gözlerim Yolda Gelme Deli Deli Rabbim Ne İdim Ne Oldum Değme Doktor Değme Yaram Başkadır Arzuhalim Vardır', '340.00', 59, 11, '/Gorsel/be21257e-ca52-4519-88c3-bad6241d933a.png'),
(19, 'Neşet Ertaş - Zahidem (PLAK)', 'Neşet Ertaş - Zahidem / Gönül Dağı - Plak A Yüzü 1- Gönül Dağı 2- Zahidem 3- Kar Yağar Kar Üstüne 4- Sanki Sam Yelisin Estin Bağıma 5- Ne Güzel Yaratmış Seni Yaradan 6- Haydar Haydar B Yüzü 1- Gönül Arzu Ediyor Yari Görmeyi 2- Tatlı Dile Güler Yüze Doyulur Mu 3- Küstürdün Gönülü 4- Sevsem Öldürürler Sevmesem Öldüm 5- Amanın Leyla', '440.00', 25, 11, '/Gorsel/6d6faaf2-44ae-47f4-b26e-48f8fcec0b70.png'),
(20, 'Gülden Karaböcek Dilek Taşı Plak (orijinal 45\'lik Plak Kayıtları)', 'LP:1A 1. Dilek Taşı 2. Nem Kaldı 3. Bitmez Tükenmez Geceler 4. Gitme Turnam 5. Şu Sazıma Bir Düzen Ver 6. Dokunma Keyfine Yalan Dünyanın 7. Bana Dışarıdan Gülen Dostlarım LP:1B 1. Sevmek Nedir Ki 2. Yalancısın 3. Ahu Gözlüm Ve Diğerleri', '390.00', 65, 12, '/Gorsel/bc29552a-ca52-4519-88c3-bad6241d933a.png'),
(21, 'FERDİ ÖZBEĞEN SİZİN SEÇTİKLERİNİZLE 33 LÜK PLAK', '1. Beni Hatırla 2. Bunca Güzel İçinde 3. Hayatım Dertlere Köle 4. Eski Dostlar 5. Eskisi Gibi 6. Şarkılardan Fal Tuttum 7. Mutluluklar Dilerim 8. Bir Sevgili Bulamadım 9. Dönsen Bile 10. Su Akar 11. Bağ Ayrı', '340.00', 97, 12, '/Gorsel/cb01ae8b-b0e1-493f-bf7a-36bf75b959a8.png'),
(22, 'Plak - Çeşitli Sanatçılar / Bergen Saygı Albümü', ' 1. Bade Derinöz – Aşk Kitabı 2. Ceylan Ertem – Benim İçin Üzülme 3. Derya Uluğ – Canım Dediklerim 4. Feride Hilal Akın – Elimde Duran Fotoğrafın 5. Gülşen – Sen Affetsen Ben Affetmem 6. Jehan Barbur – Sabır Ver 7. Melek Mosso – Yıllar Affetmez 8. Melike Şahin – Kader Diyemezsin', '599.00', 85, 12, '/Gorsel/da00c653-3a8b-45f5-882a-e9fbcbbd930e.png'),
(23, 'Cem Karaca - The Best Of (lp)', 'A YÜZÜ 1 - NAMUS BELASI 2 - GEL GEL 3 - KENDİM ETTİM KENDİM BULDUM 4 - İSTANBUL\'U DİNLİYORUM 5 - O LEYLİ 6 - EDALI GELİN B YÜZÜ 1 - TAMİRCİ ÇIRAĞI 2 - OBUR DÜNYA 3 - NİKSAR 4 - KARACAOĞLAN 5 - ÜRYAN GELDİM 6 - OĞLUMA', '460.00', 200, 14, '/Gorsel/cc0931f3-a9ff-400f-9166-46aa17801e11.png'),
(25, 'PLAK - Barış Manço / Hal Hal (2LP)', 'LP1A Hal hal Dönence Alla beni pulla beni Ademoğlu kızgın fırın LP1B Ali yazar veli bozar Bahçede hanımeli Gülpembe Halil ibrahim sofrası LP2A Geçti dost kervanı Kazma Bal sultan Aman yavaş aheste LP2B Kol düğmeleri Eski bir fincan Çıt çıt çedene Arkadaşım eşşek', '609.00', 250, 14, '/Gorsel/e1fae3cf-6824-44c5-a0c8-025edc9161c1.png'),
(39, 'Erkin Koray Fesuphanallah Plak', 'Erkin Korayın Fesuphanallah albümü 33lük plak formatında dinleyicilerle buluşuyor. Bu plağı pikabınızda en iyi ses kalitesiyle dinleyebilirsiniz.', '390.00', 119, 14, '/Gorsel/Ekran görüntüsü 2024-03-18 015104.png');

--
-- Dökümü yapılmış tablolar için indeksler
--

--
-- Tablo için indeksler `hakkimizdat`
--
ALTER TABLE `hakkimizdat`
  ADD PRIMARY KEY (`HakkimizdaID`);

--
-- Tablo için indeksler `iletisimt`
--
ALTER TABLE `iletisimt`
  ADD PRIMARY KEY (`MesajID`);

--
-- Tablo için indeksler `kategorilert`
--
ALTER TABLE `kategorilert`
  ADD PRIMARY KEY (`KategoriID`);

--
-- Tablo için indeksler `kullanicilart`
--
ALTER TABLE `kullanicilart`
  ADD PRIMARY KEY (`KullaniciID`);

--
-- Tablo için indeksler `siparisdetayt`
--
ALTER TABLE `siparisdetayt`
  ADD PRIMARY KEY (`DetayID`);

--
-- Tablo için indeksler `siparislert`
--
ALTER TABLE `siparislert`
  ADD PRIMARY KEY (`SiparisID`);

--
-- Tablo için indeksler `urunlert`
--
ALTER TABLE `urunlert`
  ADD PRIMARY KEY (`UrunID`);

--
-- Dökümü yapılmış tablolar için AUTO_INCREMENT değeri
--

--
-- Tablo için AUTO_INCREMENT değeri `hakkimizdat`
--
ALTER TABLE `hakkimizdat`
  MODIFY `HakkimizdaID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Tablo için AUTO_INCREMENT değeri `iletisimt`
--
ALTER TABLE `iletisimt`
  MODIFY `MesajID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Tablo için AUTO_INCREMENT değeri `kategorilert`
--
ALTER TABLE `kategorilert`
  MODIFY `KategoriID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- Tablo için AUTO_INCREMENT değeri `kullanicilart`
--
ALTER TABLE `kullanicilart`
  MODIFY `KullaniciID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=140;

--
-- Tablo için AUTO_INCREMENT değeri `siparisdetayt`
--
ALTER TABLE `siparisdetayt`
  MODIFY `DetayID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- Tablo için AUTO_INCREMENT değeri `siparislert`
--
ALTER TABLE `siparislert`
  MODIFY `SiparisID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- Tablo için AUTO_INCREMENT değeri `urunlert`
--
ALTER TABLE `urunlert`
  MODIFY `UrunID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
