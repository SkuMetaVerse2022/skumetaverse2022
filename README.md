# SKU_backup

<hr>

## gitignore 주의사항

gitignore에 PDF 리더 파일 3개가 ignore되어 있음 주의바람

대상은 아래의 세 파일
SKU_MetaVerse2022/Assets/Paroxe/PDFRenderer/Plugins/iOS/pdfrenderer.a
SKU_MetaVerse2022/Assets/Vuplex/WebView/Plugins/Windows/VuplexWebViewChromium/libcef.dll

## ignore 세부사항
<hr>

/[Aa]ssets/Paroxe/PDFRenderer/Plugins/iOS/pdfrenderer.a
/[Aa]ssets/Vuplex/WebView/Plugins/Windows/VuplexWebViewChromium/libcef.dll
/[Aa]ssets/Vuplex/WebView/Plugins/Windows/libcef_with_codecs.dll
이상 PDF 리더에 적용 이그노어

/[Aa]ssets/06.Images/UI/MetaVerseLoginPage(Photoshop blur).png
/[Aa]ssets/06.Images/UI/LoginBackgroundImage.png
/[Aa]ssets/TextMesh Pro/Resources/Fonts & Materials/NanumSquare_acB SDF.asset
이상 디자인에 적용된 이그노어

/[Aa]ssets/01.Scene/RoomModern/NavMesh.asset
이상 Navmesh asset에 적용된 이그노어
