Imports PdfSharp.Fonts

Public Class CustomFontResolver
    'Implements IFontResolver


    'Private ReadOnly _fontFamilies As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
    '    {"Arial", "Arial"},
    '    {"Arial Bold", "Arial Bold"},
    '    {"Arial Italic", "Arial Italic"},
    '    {"Arial Bold Italic", "Arial Bold Italic"}
    '}

    'Public Function ResolveTypeface(familyName As String, isBold As Boolean, isItalic As Boolean) As FontResolverInfo
    '    Dim key As String = familyName
    '    If isBold Then key &= " Bold"
    '    If isItalic Then key &= If(isBold, " Italic", " Italic")


    '    Return If(_fontFamilies.ContainsKey(key), _fontFamilies(key), "Arial")
    'End Function


    'Public Function GetFont(faceName As String) As Byte
    '    ' Здесь можно загружать шрифты из ресурсов/файлов, если нужно
    '    ' Для простоты используем системные шрифты
    '    Return Nothing
    'End Function
End Class
