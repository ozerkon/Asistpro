using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace SgkAssistant.Helpers
{
    public class Localization : RadGridLocalizationProvider
    {

        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadGridStringId.ConditionalFormattingPleaseSelectValidCellValue: return "Lütfen Geçerli Bir Hücre Seçiniz";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValue: return "Lütfen Geçerli Bir Hücre Giriniz";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValues: return "Lütfen Geçerli Hücreler Giriniz";
                case RadGridStringId.ConditionalFormattingPleaseSetValidExpression: return "Lütfen Geçerli bir İfade Giriniz";
                case RadGridStringId.ConditionalFormattingItem: return "Öge";
                case RadGridStringId.ConditionalFormattingInvalidParameters: return "Geçersiz Öge";
                case RadGridStringId.FilterFunctionBetween: return "Arsında";
                case RadGridStringId.FilterFunctionContains: return "Ara";
                case RadGridStringId.FilterFunctionDoesNotContain: return "İçermez";
                case RadGridStringId.FilterFunctionEndsWith: return "... İle Biten";
                case RadGridStringId.FilterFunctionEqualTo: return "Eşittir";
                case RadGridStringId.FilterFunctionGreaterThan: return "Büyüktür";
                case RadGridStringId.FilterFunctionGreaterThanOrEqualTo: return "Büyük ya da Eşittir";
                case RadGridStringId.FilterFunctionIsEmpty: return "Boş Olanlar";
                case RadGridStringId.FilterFunctionIsNull: return "Boş Olanlar";
                case RadGridStringId.FilterFunctionLessThan: return "Küçüktür";
                case RadGridStringId.FilterFunctionLessThanOrEqualTo: return "Küçük ya da Eşittir";
                case RadGridStringId.FilterFunctionNoFilter: return "Filtreyi Kaldır";
                case RadGridStringId.FilterFunctionNotBetween: return "Arasında Değildir";
                case RadGridStringId.FilterFunctionNotEqualTo: return "Eşit Değildir";
                case RadGridStringId.FilterFunctionNotIsEmpty: return "Boş Olmayanlar";
                case RadGridStringId.FilterFunctionNotIsNull: return "Boş Olmayanlar";
                case RadGridStringId.FilterFunctionStartsWith: return "... İle Başlayanlar";
                case RadGridStringId.FilterFunctionCustom: return "Özel filtre";
                case RadGridStringId.FilterOperatorBetween: return "Arasında";
                case RadGridStringId.FilterOperatorContains: return "Ara";
                case RadGridStringId.FilterOperatorDoesNotContain: return "İçermez";
                case RadGridStringId.FilterOperatorEndsWith: return "... İle Bitenler";
                case RadGridStringId.FilterOperatorEqualTo: return "Eşittir";
                case RadGridStringId.FilterOperatorGreaterThan: return "Büyüktür";
                case RadGridStringId.FilterOperatorGreaterThanOrEqualTo: return "Büyük ya da Eşittir";
                case RadGridStringId.FilterOperatorIsEmpty: return "Boş Olanlar";
                case RadGridStringId.FilterOperatorIsNull: return "Boş Olanlar";
                case RadGridStringId.FilterOperatorLessThan: return "Küçüktür";
                case RadGridStringId.FilterOperatorLessThanOrEqualTo: return "Küçük ya da Eşittir";
                case RadGridStringId.FilterOperatorNoFilter: return "Filtre Yok";
                case RadGridStringId.FilterOperatorNotBetween: return "Arasında Değildir";
                case RadGridStringId.FilterOperatorNotEqualTo: return "Eşit Değildir";
                case RadGridStringId.FilterOperatorNotIsEmpty: return "Boş Olmayanlar";
                case RadGridStringId.FilterOperatorNotIsNull: return "Değer Girilenler";
                case RadGridStringId.FilterOperatorStartsWith: return "...İle Başlayanlar";
                case RadGridStringId.FilterOperatorIsLike: return "Benzeyen";
                case RadGridStringId.FilterOperatorNotIsLike: return "Benzemeyen";
                case RadGridStringId.FilterOperatorIsContainedIn: return "İçinde Bulunan";
                case RadGridStringId.FilterOperatorNotIsContainedIn: return "İçinde Bulunmayan";
                case RadGridStringId.FilterOperatorCustom: return "Özel";
                case RadGridStringId.CustomFilterMenuItem: return "Özel Filtre";
                case RadGridStringId.CustomFilterDialogCaption: return "{0} Sütunu için özel filtre";
                case RadGridStringId.CustomFilterDialogLabel: return "Özel Filtre Seçiniz";
                case RadGridStringId.CustomFilterDialogRbAnd: return "Ve";
                case RadGridStringId.CustomFilterDialogRbOr: return "Veya";
                case RadGridStringId.CustomFilterDialogBtnOk: return "Tamam";
                case RadGridStringId.CustomFilterDialogBtnCancel: return "İptal";
                case RadGridStringId.CustomFilterDialogCheckBoxNot: return "Değil";
                case RadGridStringId.CustomFilterDialogTrue: return "Doğru";
                case RadGridStringId.CustomFilterDialogFalse: return "Yanlış";
                case RadGridStringId.FilterMenuBlanks: return "Boşluklar";
                case RadGridStringId.FilterMenuAvailableFilters: return "Geçerli Filtreler";
                case RadGridStringId.FilterMenuSearchBoxText: return "Ara";
                case RadGridStringId.FilterMenuClearFilters: return "Filtreleri Temizle";
                case RadGridStringId.FilterMenuButtonOK: return "Tamam";
                case RadGridStringId.FilterMenuButtonCancel: return "İptal";
                case RadGridStringId.FilterMenuSelectionAll: return "Hepsini Seç";
                case RadGridStringId.FilterMenuSelectionAllSearched: return "Tüm Arama Sonuçları";
                case RadGridStringId.FilterMenuSelectionNull: return "Değer girilmemiş";
                case RadGridStringId.FilterMenuSelectionNotNull: return "Değer girilmiş";
                case RadGridStringId.FilterFunctionSelectedDates: return "Belirli Tarihlere Göre Filtrele:";
                case RadGridStringId.FilterFunctionToday: return "Bugün";
                case RadGridStringId.FilterFunctionYesterday: return "Dün";
                case RadGridStringId.FilterFunctionDuringLast7days: return "Son 7 gün içinde";
                case RadGridStringId.FilterLogicalOperatorAnd: return "Ve";
                case RadGridStringId.FilterLogicalOperatorOr: return "Veya";
                case RadGridStringId.FilterCompositeNotOperator: return "Değil";
                case RadGridStringId.DeleteRowMenuItem: return "Satırı sil";
                case RadGridStringId.SortAscendingMenuItem: return "Artan Sıralama";
                case RadGridStringId.SortDescendingMenuItem: return "Azalan Sıralama";
                case RadGridStringId.ClearSortingMenuItem: return "Sıralamayı Kaldır";
                case RadGridStringId.ConditionalFormattingMenuItem: return "Koşullu Biçimlendirme";
                case RadGridStringId.GroupByThisColumnMenuItem: return "Sütuna Göre Gruplandır";
                case RadGridStringId.UngroupThisColumn: return "Sütun Grubunu Çöz";
                case RadGridStringId.ColumnChooserMenuItem: return "Sütun Seçici";
                case RadGridStringId.HideMenuItem: return "Sütunu Gizle";
                case RadGridStringId.HideGroupMenuItem: return "Grubu Gizle";
                case RadGridStringId.UnpinMenuItem: return "Sütunu Çöz";
                case RadGridStringId.UnpinRowMenuItem: return "Satırı Çöz";
                case RadGridStringId.PinMenuItem: return "Sabitle";
                case RadGridStringId.PinAtLeftMenuItem: return "Sola Sabitle";
                case RadGridStringId.PinAtRightMenuItem: return "Sağa Sabitle";
                case RadGridStringId.PinAtBottomMenuItem: return "Alta Sabitle";
                case RadGridStringId.PinAtTopMenuItem: return "Üste Sabitle";
                case RadGridStringId.BestFitMenuItem: return "En Uygun Genişlik";
                case RadGridStringId.PasteMenuItem: return "Yapıştır";
                case RadGridStringId.EditMenuItem: return "Düzenle";
                case RadGridStringId.ClearValueMenuItem: return "İçeriği Temizle";
                case RadGridStringId.CopyMenuItem: return "Kopyala";
                case RadGridStringId.CutMenuItem: return "Kes";
                case RadGridStringId.AddNewRowString: return "Yeni Satır Eklemek İçin Tıklayın";
                case RadGridStringId.ConditionalFormattingSortAlphabetically: return "Sütunları Alfabetik Sırala";
                case RadGridStringId.ConditionalFormattingCaption: return "Koşullu Biçimlendirme Kural Yöneticisi";
                case RadGridStringId.ConditionalFormattingLblColumn: return "Yalnızca Şu Özelliklere Sahip Hücreleri Biçimlendir";
                case RadGridStringId.ConditionalFormattingLblName: return "Kural Adı";
                case RadGridStringId.ConditionalFormattingLblType: return "Hücre Değeri";
                case RadGridStringId.ConditionalFormattingLblValue1: return "Değer 1";
                case RadGridStringId.ConditionalFormattingLblValue2: return "Değer 2";
                case RadGridStringId.ConditionalFormattingGrpConditions: return "Kurallar";
                case RadGridStringId.ConditionalFormattingGrpProperties: return "Kural Özellikleri";
                case RadGridStringId.ConditionalFormattingChkApplyToRow: return "Biçimlendirmeyi Bütün Satıra Uygula";
                case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows: return "Biçimlendirmeyi Seçili Satırlara Uygula";
                case RadGridStringId.ConditionalFormattingBtnAdd: return "Yeni Kural Ekle";
                case RadGridStringId.ConditionalFormattingBtnRemove: return "Kaldır";
                case RadGridStringId.ConditionalFormattingBtnOK: return "Tamam";
                case RadGridStringId.ConditionalFormattingBtnCancel: return "İptal";
                case RadGridStringId.ConditionalFormattingBtnApply: return "Uygula";
                case RadGridStringId.ConditionalFormattingRuleAppliesOn: return "Uygulanacak Sütunlar:";
                case RadGridStringId.ConditionalFormattingCondition: return "Koşul";
                case RadGridStringId.ConditionalFormattingExpression: return "İfade";
                case RadGridStringId.ConditionalFormattingChooseOne: return "[Birini seçin]";
                case RadGridStringId.ConditionalFormattingEqualsTo: return "Eşittir [Değer1]";
                case RadGridStringId.ConditionalFormattingIsNotEqualTo: return "Eşit Değildir [Değer1]";
                case RadGridStringId.ConditionalFormattingStartsWith: return "... İle Başlar [Değer1]";
                case RadGridStringId.ConditionalFormattingEndsWith: return "... İle Biter [Değer1]";
                case RadGridStringId.ConditionalFormattingContains: return "Ara [Değer1]";
                case RadGridStringId.ConditionalFormattingDoesNotContain: return "İçermez [Değer1]";
                case RadGridStringId.ConditionalFormattingIsGreaterThan: return "Büyüktür [Değer1]";
                case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual: return "Büyk ya da Eşittir [Değer1]";
                case RadGridStringId.ConditionalFormattingIsLessThan: return "Küçüktür [Değer1]";
                case RadGridStringId.ConditionalFormattingIsLessThanOrEqual: return "Küçük ya da Eşittir [Değer1]";
                case RadGridStringId.ConditionalFormattingIsBetween: return "[Değer1] ile [Değer2] Arasındadır";
                case RadGridStringId.ConditionalFormattingIsNotBetween: return "[Değer1] ile [Değer2] Arasında Değildir";
                case RadGridStringId.ConditionalFormattingLblFormat: return "Biçim";
                case RadGridStringId.ConditionalFormattingBtnExpression: return "İfade Oluşturucu";
                case RadGridStringId.ConditionalFormattingTextBoxExpression: return "İfade";
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive: return "Büyük-Küçük Harf Duyarlı";
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor: return "Hücre Arkaplan Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor: return "Hücre Metin Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridEnabled: return "Etkin";
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor: return "Satır Arkaplan Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor: return "Satır Metin Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment: return "Satır Metin Hizalama";
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment: return "Metin Hizalama";
                case RadGridStringId.ConditionalFormattingPropertyGridCellFont: return "Hücre Yazı Tipi";
                case RadGridStringId.ConditionalFormattingPropertyGridCellFontDescription: return "Yazı Tipi Açıklaması";
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription: return "Dize Değerleri Değerlendirilirken Büyük / Küçük Harfe Duyarlı Karşılaştırmaların Yapılıp Yapılmayacağını Belirler.";
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription: return "Hücre İçin Kullanılacak Arka Plan Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription: return "Hücre İçin Kullanılacak Metin Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription: return "Koşulun Etkin Olup Olmadığını Belirler (Değerlendirilebilir ve Uygulanabilir).";
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription: return "Tüm Satır İçin Kullanılacak Arka Plan Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription: return "Tüm Satır İçin Kullanılacak Metin Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription: return "\"Satıra Uygula\" Evet Olduğunda, Hücre Değerleri İçin Kullanılacak Hizalamayı Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription: return "Hücre Değerleri İçin Kullanılacak Hizalamayı Girin.";
                case RadGridStringId.ColumnChooserFormCaption: return "Sütun Seçici";
                case RadGridStringId.ColumnChooserFormMessage: return "Geçerli Görünümden Kaldırmak İçin Bir \nSütun Başlığını Izgaradan Buraya Sürükleyin.";
                case RadGridStringId.GroupingPanelDefaultMessage: return "Gruplandırmak İstediğiniz Sütunu Buraya Sürükleyin";
                case RadGridStringId.GroupingPanelHeader: return "Şuna Göre Grupla: ";
                case RadGridStringId.PagingPanelPagesLabel: return "Sayfa ";
                case RadGridStringId.PagingPanelOfPagesLabel: return " /";
                case RadGridStringId.NoDataText: return "İşlem süresi, internet bağlantı kalitenize, taranacak firma sayısına ve Sgk sunucularının yoğunluna bağlı olarak değişiklik gösterebilir.";
                case RadGridStringId.CompositeFilterFormErrorCaption: return "Filtre Hatası";
                case RadGridStringId.CompositeFilterFormInvalidFilter: return "Bileşik Filtre Tanımlayıcı Geçerli Değil.";
                case RadGridStringId.ExpressionMenuItem: return "İfade";
                case RadGridStringId.ExpressionFormTitle: return "İfade Oluşturucu";
                case RadGridStringId.ExpressionFormFunctions: return "İşlevler";
                case RadGridStringId.ExpressionFormFunctionsText: return "Metin";
                case RadGridStringId.ExpressionFormFunctionsAggregate: return "ToplamA";
                case RadGridStringId.ExpressionFormFunctionsDateTime: return "Tarih-Zaman";
                case RadGridStringId.ExpressionFormFunctionsLogical: return "Mantıksal";
                case RadGridStringId.ExpressionFormFunctionsMath: return "Matematik";
                case RadGridStringId.ExpressionFormFunctionsOther: return "Diğer";
                case RadGridStringId.ExpressionFormOperators: return "Operatörler";
                case RadGridStringId.ExpressionFormConstants: return "Sabitler";
                case RadGridStringId.ExpressionFormFields: return "Alanlar";
                case RadGridStringId.ExpressionFormDescription: return "Açıklama";
                case RadGridStringId.ExpressionFormResultPreview: return "Sonuç Ön İzleme";
                case RadGridStringId.ExpressionFormTooltipPlus: return "Artı";
                case RadGridStringId.ExpressionFormTooltipMinus: return "Eksi";
                case RadGridStringId.ExpressionFormTooltipMultiply: return "Çarpım";
                case RadGridStringId.ExpressionFormTooltipDivide: return "Bölme";
                case RadGridStringId.ExpressionFormTooltipModulo: return "Bölme İşleminden Kalanı Bulma";
                case RadGridStringId.ExpressionFormTooltipEqual: return "Eşittir";
                case RadGridStringId.ExpressionFormTooltipNotEqual: return "Eşit Değildir";
                case RadGridStringId.ExpressionFormTooltipLess: return "Küçüktür";
                case RadGridStringId.ExpressionFormTooltipLessOrEqual: return "Küçük ya da Eşittir";
                case RadGridStringId.ExpressionFormTooltipGreaterOrEqual: return "Büyük ya da Eşittir";
                case RadGridStringId.ExpressionFormTooltipGreater: return "Büyüktür";
                case RadGridStringId.ExpressionFormTooltipAnd: return "Mantıksal \"VE\"";
                case RadGridStringId.ExpressionFormTooltipOr: return "Mantıksal \"VEYA\"";
                case RadGridStringId.ExpressionFormTooltipNot: return "Mantıksal \"DEĞİL\"";
                case RadGridStringId.ExpressionFormAndButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOrButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormNotButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOKButton: return "Tamam";
                case RadGridStringId.ExpressionFormCancelButton: return "İptal";
                case RadGridStringId.SearchRowChooseColumns: return "Arama Satırı Sütunları Seçin";
                case RadGridStringId.SearchRowSearchFromCurrentPosition: return "Aramaya Seçili Satırdan Başla";
                case RadGridStringId.SearchRowMenuItemMasterTemplate: return "Arama Satırı Menü Öğesi Ana Şablonu";
                case RadGridStringId.SearchRowMenuItemChildTemplates: return "Arama Satırı Menü Öğesi Alt Şablonları";
                case RadGridStringId.SearchRowMenuItemAllColumns: return "Arama Satırı Menü Öğesi Tüm Sütunları";
                case RadGridStringId.SearchRowTextBoxNullText: return "Aranacak Kelimeyi Yazın";
                case RadGridStringId.SearchRowResultsOfLabel: return "Etiketin Arama Satırı Sonuçları";
                case RadGridStringId.SearchRowMatchCase: return "Büyük-küçük harf eşleştir";
            }
            return string.Empty;
        }


    }
    public class LocalizationForImport : RadGridLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadGridStringId.ConditionalFormattingPleaseSelectValidCellValue: return "Lütfen Geçerli Bir Hücre Seçiniz";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValue: return "Lütfen Geçerli Bir Hücre Giriniz";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValues: return "Lütfen Geçerli Hücreler Giriniz";
                case RadGridStringId.ConditionalFormattingPleaseSetValidExpression: return "Lütfen Geçerli bir İfade Giriniz";
                case RadGridStringId.ConditionalFormattingItem: return "Öge";
                case RadGridStringId.ConditionalFormattingInvalidParameters: return "Geçersiz Öge";
                case RadGridStringId.FilterFunctionBetween: return "Arsında";
                case RadGridStringId.FilterFunctionContains: return "Ara";
                case RadGridStringId.FilterFunctionDoesNotContain: return "İçermez";
                case RadGridStringId.FilterFunctionEndsWith: return "... İle Biten";
                case RadGridStringId.FilterFunctionEqualTo: return "Eşittir";
                case RadGridStringId.FilterFunctionGreaterThan: return "Büyüktür";
                case RadGridStringId.FilterFunctionGreaterThanOrEqualTo: return "Büyük ya da Eşittir";
                case RadGridStringId.FilterFunctionIsEmpty: return "Boş Olanlar";
                case RadGridStringId.FilterFunctionIsNull: return "Null Olanlar";
                case RadGridStringId.FilterFunctionLessThan: return "Küçüktür";
                case RadGridStringId.FilterFunctionLessThanOrEqualTo: return "Küçük ya da Eşittir";
                case RadGridStringId.FilterFunctionNoFilter: return "Filtreyi Kaldır";
                case RadGridStringId.FilterFunctionNotBetween: return "Arasında Değildir";
                case RadGridStringId.FilterFunctionNotEqualTo: return "Eşit Değildir";
                case RadGridStringId.FilterFunctionNotIsEmpty: return "Boş Olmayanlar";
                case RadGridStringId.FilterFunctionNotIsNull: return "Null Olmayanlar";
                case RadGridStringId.FilterFunctionStartsWith: return "... İle Başlayanlar";
                case RadGridStringId.FilterFunctionCustom: return "Özel filtre";
                case RadGridStringId.FilterOperatorBetween: return "Arasında";
                case RadGridStringId.FilterOperatorContains: return "Ara";
                case RadGridStringId.FilterOperatorDoesNotContain: return "İçermez";
                case RadGridStringId.FilterOperatorEndsWith: return "... İle Bitenler";
                case RadGridStringId.FilterOperatorEqualTo: return "Eşittir";
                case RadGridStringId.FilterOperatorGreaterThan: return "Büyüktür";
                case RadGridStringId.FilterOperatorGreaterThanOrEqualTo: return "Büyük ya da Eşittir";
                case RadGridStringId.FilterOperatorIsEmpty: return "Boş Olanlar";
                case RadGridStringId.FilterOperatorIsNull: return "Null Olanlar";
                case RadGridStringId.FilterOperatorLessThan: return "Küçüktür";
                case RadGridStringId.FilterOperatorLessThanOrEqualTo: return "Küçük ya da Eşittir";
                case RadGridStringId.FilterOperatorNoFilter: return "Filtre Yok";
                case RadGridStringId.FilterOperatorNotBetween: return "Arasında Değildir";
                case RadGridStringId.FilterOperatorNotEqualTo: return "Eşit Değildir";
                case RadGridStringId.FilterOperatorNotIsEmpty: return "Boş Olmayanlar";
                case RadGridStringId.FilterOperatorNotIsNull: return "Değer Girilenler";
                case RadGridStringId.FilterOperatorStartsWith: return "...İle Başlayanlar";
                case RadGridStringId.FilterOperatorIsLike: return "Benzeyen";
                case RadGridStringId.FilterOperatorNotIsLike: return "Benzemeyen";
                case RadGridStringId.FilterOperatorIsContainedIn: return "İçinde Bulunan";
                case RadGridStringId.FilterOperatorNotIsContainedIn: return "İçinde Bulunmayan";
                case RadGridStringId.FilterOperatorCustom: return "Özel";
                case RadGridStringId.CustomFilterMenuItem: return "Özel Filtre";
                case RadGridStringId.CustomFilterDialogCaption: return "{0} Sütunu için özel filtre";
                case RadGridStringId.CustomFilterDialogLabel: return "Özel Filtre Seçiniz";
                case RadGridStringId.CustomFilterDialogRbAnd: return "Ve";
                case RadGridStringId.CustomFilterDialogRbOr: return "Veya";
                case RadGridStringId.CustomFilterDialogBtnOk: return "Tamam";
                case RadGridStringId.CustomFilterDialogBtnCancel: return "İptal";
                case RadGridStringId.CustomFilterDialogCheckBoxNot: return "Değil";
                case RadGridStringId.CustomFilterDialogTrue: return "Doğru";
                case RadGridStringId.CustomFilterDialogFalse: return "Yanlış";
                case RadGridStringId.FilterMenuBlanks: return "Boşluklar";
                case RadGridStringId.FilterMenuAvailableFilters: return "Geçerli Filtreler";
                case RadGridStringId.FilterMenuSearchBoxText: return "Ara";
                case RadGridStringId.FilterMenuClearFilters: return "Filtreleri Temizle";
                case RadGridStringId.FilterMenuButtonOK: return "Tamam";
                case RadGridStringId.FilterMenuButtonCancel: return "İptal";
                case RadGridStringId.FilterMenuSelectionAll: return "Hepsini Seç";
                case RadGridStringId.FilterMenuSelectionAllSearched: return "Tüm Arama Sonuçları";
                case RadGridStringId.FilterMenuSelectionNull: return "Değer girilmemiş";
                case RadGridStringId.FilterMenuSelectionNotNull: return "Değer girilmiş";
                case RadGridStringId.FilterFunctionSelectedDates: return "Belirli Tarihlere Göre Filtrele:";
                case RadGridStringId.FilterFunctionToday: return "Bugün";
                case RadGridStringId.FilterFunctionYesterday: return "Dün";
                case RadGridStringId.FilterFunctionDuringLast7days: return "Son 7 gün içinde";
                case RadGridStringId.FilterLogicalOperatorAnd: return "Ve";
                case RadGridStringId.FilterLogicalOperatorOr: return "Veya";
                case RadGridStringId.FilterCompositeNotOperator: return "Değil";
                case RadGridStringId.DeleteRowMenuItem: return "Satırı sil";
                case RadGridStringId.SortAscendingMenuItem: return "Artan Sıralama";
                case RadGridStringId.SortDescendingMenuItem: return "Azalan Sıralama";
                case RadGridStringId.ClearSortingMenuItem: return "Sıralamayı Kaldır";
                case RadGridStringId.ConditionalFormattingMenuItem: return "Koşullu Biçimlendirme";
                case RadGridStringId.GroupByThisColumnMenuItem: return "Sütuna Göre Gruplandır";
                case RadGridStringId.UngroupThisColumn: return "Sütun Grubunu Çöz";
                case RadGridStringId.ColumnChooserMenuItem: return "Sütun Seçici";
                case RadGridStringId.HideMenuItem: return "Sütunu Gizle";
                case RadGridStringId.HideGroupMenuItem: return "Grubu Gizle";
                case RadGridStringId.UnpinMenuItem: return "Sütunu Çöz";
                case RadGridStringId.UnpinRowMenuItem: return "Satırı Çöz";
                case RadGridStringId.PinMenuItem: return "Sabitle";
                case RadGridStringId.PinAtLeftMenuItem: return "Sola Sabitle";
                case RadGridStringId.PinAtRightMenuItem: return "Sağa Sabitle";
                case RadGridStringId.PinAtBottomMenuItem: return "Alta Sabitle";
                case RadGridStringId.PinAtTopMenuItem: return "Üste Sabitle";
                case RadGridStringId.BestFitMenuItem: return "En Uygun Genişlik";
                case RadGridStringId.PasteMenuItem: return "Yapıştır";
                case RadGridStringId.EditMenuItem: return "Düzenle";
                case RadGridStringId.ClearValueMenuItem: return "İçeriği Temizle";
                case RadGridStringId.CopyMenuItem: return "Kopyala";
                case RadGridStringId.CutMenuItem: return "Kes";
                case RadGridStringId.AddNewRowString: return "Yeni Satır Eklemek İçin Tıklayın";
                case RadGridStringId.ConditionalFormattingSortAlphabetically: return "Sütunları Alfabetik Sırala";
                case RadGridStringId.ConditionalFormattingCaption: return "Koşullu Biçimlendirme Kural Yöneticisi";
                case RadGridStringId.ConditionalFormattingLblColumn: return "Yalnızca Şu Özelliklere Sahip Hücreleri Biçimlendir";
                case RadGridStringId.ConditionalFormattingLblName: return "Kural Adı";
                case RadGridStringId.ConditionalFormattingLblType: return "Hücre Değeri";
                case RadGridStringId.ConditionalFormattingLblValue1: return "Değer 1";
                case RadGridStringId.ConditionalFormattingLblValue2: return "Değer 2";
                case RadGridStringId.ConditionalFormattingGrpConditions: return "Kurallar";
                case RadGridStringId.ConditionalFormattingGrpProperties: return "Kural Özellikleri";
                case RadGridStringId.ConditionalFormattingChkApplyToRow: return "Biçimlendirmeyi Bütün Satıra Uygula";
                case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows: return "Biçimlendirmeyi Seçili Satırlara Uygula";
                case RadGridStringId.ConditionalFormattingBtnAdd: return "Yeni Kural Ekle";
                case RadGridStringId.ConditionalFormattingBtnRemove: return "Kaldır";
                case RadGridStringId.ConditionalFormattingBtnOK: return "Tamam";
                case RadGridStringId.ConditionalFormattingBtnCancel: return "İptal";
                case RadGridStringId.ConditionalFormattingBtnApply: return "Uygula";
                case RadGridStringId.ConditionalFormattingRuleAppliesOn: return "Kural Şunlar İçin Geçerlidir:";
                case RadGridStringId.ConditionalFormattingCondition: return "Koşul";
                case RadGridStringId.ConditionalFormattingExpression: return "İfade";
                case RadGridStringId.ConditionalFormattingChooseOne: return "[Birini seçin]";
                case RadGridStringId.ConditionalFormattingEqualsTo: return "Eşittir [Değer1]";
                case RadGridStringId.ConditionalFormattingIsNotEqualTo: return "Eşit Değildir [Değer1]";
                case RadGridStringId.ConditionalFormattingStartsWith: return "... İle Başlar [Değer1]";
                case RadGridStringId.ConditionalFormattingEndsWith: return "... İle Biter [Değer1]";
                case RadGridStringId.ConditionalFormattingContains: return "Ara [Değer1]";
                case RadGridStringId.ConditionalFormattingDoesNotContain: return "İçermez [Değer1]";
                case RadGridStringId.ConditionalFormattingIsGreaterThan: return "Büyüktür [Değer1]";
                case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual: return "Büyk ya da Eşittir [Değer1]";
                case RadGridStringId.ConditionalFormattingIsLessThan: return "Küçüktür [Değer1]";
                case RadGridStringId.ConditionalFormattingIsLessThanOrEqual: return "Küçük ya da Eşittir [Değer1]";
                case RadGridStringId.ConditionalFormattingIsBetween: return "[Değer1] ile [Değer2] Arasındadır";
                case RadGridStringId.ConditionalFormattingIsNotBetween: return "[Değer1] ile [Değer2] Arasında Değildir";
                case RadGridStringId.ConditionalFormattingLblFormat: return "Biçim";
                case RadGridStringId.ConditionalFormattingBtnExpression: return "İfade Oluşturucu";
                case RadGridStringId.ConditionalFormattingTextBoxExpression: return "İfade";
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive: return "Büyük-Küçük Harf Duyarlı";
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor: return "Hücre Arkaplan Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor: return "Hücre Metin Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridEnabled: return "Etkin";
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor: return "Satır Arkaplan Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor: return "Satır Metin Rengi";
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment: return "Satır Metin Hizalama";
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment: return "Metin Hizalama";
                case RadGridStringId.ConditionalFormattingPropertyGridCellFont: return "Hücre Yazı Tipi";
                case RadGridStringId.ConditionalFormattingPropertyGridCellFontDescription: return "Yazı Tipi Açıklaması";
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription: return "Dize Değerleri Değerlendirilirken Büyük / Küçük Harfe Duyarlı Karşılaştırmaların Yapılıp Yapılmayacağını Belirler.";
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription: return "Hücre İçin Kullanılacak Arka Plan Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription: return "Hücre İçin Kullanılacak Metin Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription: return "Koşulun Etkin Olup Olmadığını Belirler (Değerlendirilebilir ve Uygulanabilir).";
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription: return "Tüm Satır İçin Kullanılacak Arka Plan Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription: return "Tüm Satır İçin Kullanılacak Metin Rengini Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription: return "\"Satıra Uygula\" Evet Olduğunda, Hücre Değerleri İçin Kullanılacak Hizalamayı Girin.";
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription: return "Hücre Değerleri İçin Kullanılacak Hizalamayı Girin.";
                case RadGridStringId.ColumnChooserFormCaption: return "Sütun Seçici";
                case RadGridStringId.ColumnChooserFormMessage: return "Geçerli Görünümden Kaldırmak İçin Bir \nSütun Başlığını Izgaradan Buraya Sürükleyin.";
                case RadGridStringId.GroupingPanelDefaultMessage: return "Gruplandırmak İstediğiniz Sütunu Buraya Sürükleyin";
                case RadGridStringId.GroupingPanelHeader: return "Şuna Göre Grupla: ";
                case RadGridStringId.PagingPanelPagesLabel: return "Sayfa ";
                case RadGridStringId.PagingPanelOfPagesLabel: return " /";
                case RadGridStringId.NoDataText: return "Excel'den aldığınız veriler kaydedilmeden önce burada görüntülenir.";
                case RadGridStringId.CompositeFilterFormErrorCaption: return "Filtre Hatası";
                case RadGridStringId.CompositeFilterFormInvalidFilter: return "Bileşik Filtre Tanımlayıcı Geçerli Değil.";
                case RadGridStringId.ExpressionMenuItem: return "İfade";
                case RadGridStringId.ExpressionFormTitle: return "İfade Oluşturucu";
                case RadGridStringId.ExpressionFormFunctions: return "İşlevler";
                case RadGridStringId.ExpressionFormFunctionsText: return "Metin";
                case RadGridStringId.ExpressionFormFunctionsAggregate: return "ToplamA";
                case RadGridStringId.ExpressionFormFunctionsDateTime: return "Tarih-Zaman";
                case RadGridStringId.ExpressionFormFunctionsLogical: return "Mantıksal";
                case RadGridStringId.ExpressionFormFunctionsMath: return "Matematik";
                case RadGridStringId.ExpressionFormFunctionsOther: return "Diğer";
                case RadGridStringId.ExpressionFormOperators: return "Operatörler";
                case RadGridStringId.ExpressionFormConstants: return "Sabitler";
                case RadGridStringId.ExpressionFormFields: return "Alanlar";
                case RadGridStringId.ExpressionFormDescription: return "Açıklama";
                case RadGridStringId.ExpressionFormResultPreview: return "Sonuç Ön İzleme";
                case RadGridStringId.ExpressionFormTooltipPlus: return "Artı";
                case RadGridStringId.ExpressionFormTooltipMinus: return "Eksi";
                case RadGridStringId.ExpressionFormTooltipMultiply: return "Çarpım";
                case RadGridStringId.ExpressionFormTooltipDivide: return "Bölme";
                case RadGridStringId.ExpressionFormTooltipModulo: return "Bölme İşleminden Kalanı Bulma";
                case RadGridStringId.ExpressionFormTooltipEqual: return "Eşittir";
                case RadGridStringId.ExpressionFormTooltipNotEqual: return "Eşit Değildir";
                case RadGridStringId.ExpressionFormTooltipLess: return "Küçüktür";
                case RadGridStringId.ExpressionFormTooltipLessOrEqual: return "Küçük ya da Eşittir";
                case RadGridStringId.ExpressionFormTooltipGreaterOrEqual: return "Büyük ya da Eşittir";
                case RadGridStringId.ExpressionFormTooltipGreater: return "Büyüktür";
                case RadGridStringId.ExpressionFormTooltipAnd: return "Mantıksal \"VE\"";
                case RadGridStringId.ExpressionFormTooltipOr: return "Mantıksal \"VEYA\"";
                case RadGridStringId.ExpressionFormTooltipNot: return "Mantıksal \"DEĞİL\"";
                case RadGridStringId.ExpressionFormAndButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOrButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormNotButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOKButton: return "Tamam";
                case RadGridStringId.ExpressionFormCancelButton: return "İptal";
                case RadGridStringId.SearchRowChooseColumns: return "Arama Satırı Sütunları Seçin";
                case RadGridStringId.SearchRowSearchFromCurrentPosition: return "Geçerli Satırdan Arama Satırı Bulma";
                case RadGridStringId.SearchRowMenuItemMasterTemplate: return "Arama Satırı Menü Öğesi Ana Şablonu";
                case RadGridStringId.SearchRowMenuItemChildTemplates: return "Arama Satırı Menü Öğesi Alt Şablonları";
                case RadGridStringId.SearchRowMenuItemAllColumns: return "Arama Satırı Menü Öğesi Tüm Sütunları";
                case RadGridStringId.SearchRowTextBoxNullText: return "Arama Satırı Metin Kutusu Boş Metin";
                case RadGridStringId.SearchRowResultsOfLabel: return "Etiketin Arama Satırı Sonuçları";
                case RadGridStringId.SearchRowMatchCase: return "Büyük-küçük harf eşleştir";
            }
            return string.Empty;
        }


    }
    public class MyRadMessageLocalizationProvider : RadMessageLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadMessageStringID.AbortButton: return "Durdur";
                case RadMessageStringID.CancelButton: return "İptal";
                case RadMessageStringID.IgnoreButton: return "Yoksay";
                case RadMessageStringID.NoButton: return "Hayır";
                case RadMessageStringID.OKButton: return "Tamam";
                case RadMessageStringID.RetryButton: return "Tekrar Dene";
                case RadMessageStringID.YesButton: return "Evet";
                case RadMessageStringID.DetailsButton: return " Ayrıntılar";
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
    public class MyWizardLocalizationProvider : RadWizardLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadWizardStringId.BackButtonText: return "<   Geri";
                case RadWizardStringId.NextButtonText: return "İleri   >";
                case RadWizardStringId.CancelButtonText: return "İptal";
                case RadWizardStringId.FinishButtonText: return "Bitir";
                case RadWizardStringId.HelpButtonText: return "<html><u>Yardım</u></html>";
                default: return string.Empty;
            }
        }
    }
    public class MyPrintDialogsLocalizationProvider : Telerik.WinControls.UI.PrintDialogsLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case PrintDialogsStringId.PreviewDialogTitle: return "Baskı Önizleme";
                case PrintDialogsStringId.PreviewDialogPrint: return "Yazdır";
                case PrintDialogsStringId.PreviewDialogPrintSettings: return "Yazdırma Ayarları...";
                case PrintDialogsStringId.PreviewDialogWatermark: return "Fligran";
                case PrintDialogsStringId.PreviewDialogPreviousPage: return "Önceki Sayfa";
                case PrintDialogsStringId.PreviewDialogNextPage: return "Sonraki Sayfa";
                case PrintDialogsStringId.PreviewDialogZoomIn: return "Büyüt";
                case PrintDialogsStringId.PreviewDialogZoomOut: return "Küçült";
                case PrintDialogsStringId.PreviewDialogZoom: return "Yakınlaştır";
                case PrintDialogsStringId.PreviewDialogAuto: return "Otomatik";
                case PrintDialogsStringId.PreviewDialogLayout: return "Yerleşim";
                case PrintDialogsStringId.PreviewDialogFile: return "Dosya";
                case PrintDialogsStringId.PreviewDialogView: return "Görünüm";
                case PrintDialogsStringId.PreviewDialogTools: return "Araçlar";
                case PrintDialogsStringId.PreviewDialogExit: return "Çıkış";
                case PrintDialogsStringId.PreviewDialogStripTools: return "Araçlar";
                case PrintDialogsStringId.PreviewDialogStripNavigation: return "Gezinme";
                case PrintDialogsStringId.WatermarkDialogTitle: return "Fligran Ayarları";
                case PrintDialogsStringId.WatermarkDialogButtonOK: return "Tamam";
                case PrintDialogsStringId.WatermarkDialogButtonCancel: return "İptal";
                case PrintDialogsStringId.WatermarkDialogLabelPreview: return "Önizleme";
                case PrintDialogsStringId.WatermarkDialogButtonRemove: return "Fligranı kaldır";
                case PrintDialogsStringId.WatermarkDialogLabelPosition: return "Konum";
                case PrintDialogsStringId.WatermarkDialogRadioInFront: return "Önünde";
                case PrintDialogsStringId.WatermarkDialogRadioBehind: return "Arkasında";
                case PrintDialogsStringId.WatermarkDialogLabelPageRange: return "Sayfa aralığı";
                case PrintDialogsStringId.WatermarkDialogRadioAll: return "Hepsi";
                case PrintDialogsStringId.WatermarkDialogRadioPages: return "Sayfalar";
                case PrintDialogsStringId.WatermarkDialogLabelPagesDescription: return "(1,3,5-12 gibi)";
                case PrintDialogsStringId.WatermarkDialogTabText: return "Metin";
                case PrintDialogsStringId.WatermarkDialogTabPicture: return "Resim";
                case PrintDialogsStringId.WatermarkDialogLabelText: return "Metin";
                case PrintDialogsStringId.WatermarkDialogWatermarkText: return "Fligran metni";
                case PrintDialogsStringId.WatermarkDialogLabelHOffset: return "Yatay ofset";
                case PrintDialogsStringId.WatermarkDialogLabelVOffset: return "Dikey ofset";
                case PrintDialogsStringId.WatermarkDialogLabelRotation: return "Döndür";
                case PrintDialogsStringId.WatermarkDialogLabelFont: return "Yazı tipi:";
                case PrintDialogsStringId.WatermarkDialogLabelSize: return "Boyut:";
                case PrintDialogsStringId.WatermarkDialogLabelColor: return "Renk:";
                case PrintDialogsStringId.WatermarkDialogLabelOpacity: return "Saydamlık:";
                case PrintDialogsStringId.WatermarkDialogLabelLoadImage: return "Resim yükle:";
                case PrintDialogsStringId.WatermarkDialogCheckboxTiling: return "Tiling";
                case PrintDialogsStringId.SettingDialogTitle: return "Yazdırma ayarları";
                case PrintDialogsStringId.SettingDialogButtonPrint: return "Yazdır";
                case PrintDialogsStringId.SettingDialogButtonPreview: return "Önizleme";
                case PrintDialogsStringId.SettingDialogButtonCancel: return "İptal";
                case PrintDialogsStringId.SettingDialogButtonOK: return "Tamam";
                case PrintDialogsStringId.SettingDialogPageFormat: return "Biçim";
                case PrintDialogsStringId.SettingDialogPagePaper: return "Kağıt";
                case PrintDialogsStringId.SettingDialogPageHeaderFooter: return "Alt bilgi/Üst bilgi";
                case PrintDialogsStringId.SettingDialogButtonPageNumber: return "Sayfa numaraları";
                case PrintDialogsStringId.SettingDialogButtonTotalPages: return "Toplam sayfa sayısı";
                case PrintDialogsStringId.SettingDialogButtonCurrentDate: return "Geçerli tarih";
                case PrintDialogsStringId.SettingDialogButtonCurrentTime: return "Geçerli zaman";
                case PrintDialogsStringId.SettingDialogButtonUserName: return "Kullanıcı adı";
                case PrintDialogsStringId.SettingDialogLabelHeader: return "Üst bilgi";
                case PrintDialogsStringId.SettingDialogLabelFooter: return "Alt bilgi";
                case PrintDialogsStringId.SettingDialogCheckboxReverse: return "Çift sayfalarda ters çevir";
                case PrintDialogsStringId.SettingDialogLabelPage: return "Sayfa";
                case PrintDialogsStringId.SettingDialogLabelType: return "Tit";
                case PrintDialogsStringId.SettingDialogLabelPageSource: return "Sayfa kaynağı";
                case PrintDialogsStringId.SettingDialogLabelMargins: return "Kenar boşluğu";
                case PrintDialogsStringId.SettingDialogLabelOrientation: return "Kağıt yönü";
                case PrintDialogsStringId.SettingDialogLabelTop: return "Üst:";
                case PrintDialogsStringId.SettingDialogLabelBottom: return "Alt:";
                case PrintDialogsStringId.SettingDialogLabelLeft: return "Sol:";
                case PrintDialogsStringId.SettingDialogLabelRight: return "Sağ:";
                case PrintDialogsStringId.SettingDialogRadioPortrait: return "Dikey";
                case PrintDialogsStringId.SettingDialogRadioLandscape: return "Yatay";
                case PrintDialogsStringId.SchedulerSettingsLabelPrintStyle: return "Yazdrıma stili";
                case PrintDialogsStringId.SchedulerSettingsDailyStyle: return "Günük stil";
                case PrintDialogsStringId.SchedulerSettingsWeeklyStyle: return "Haftalık stil";
                case PrintDialogsStringId.SchedulerSettingsMonthlyStyle: return "Aylık stil";
                case PrintDialogsStringId.SchedulerSettingsDetailStyle: return "Ayrıntı stili";
                case PrintDialogsStringId.SchedulerSettingsButtonWatermark: return "Fligran...";
                case PrintDialogsStringId.SchedulerSettingsLabelPrintRange: return "Yazdırma aralığı";
                case PrintDialogsStringId.SchedulerSettingsLabelStyleSettings: return "Stil ayarları";
                case PrintDialogsStringId.SchedulerSettingsLabelPrintSettings: return "Yazdırma ayarları";
                case PrintDialogsStringId.SchedulerSettingsLabelStartDate: return "Başlangıç tarihi";
                case PrintDialogsStringId.SchedulerSettingsLabelEndDate: return "Bitiş tarihi";
                case PrintDialogsStringId.SchedulerSettingsLabelStartTime: return "Başlangıç saati";
                case PrintDialogsStringId.SchedulerSettingsLabelEndTime: return "Bitiş saati";
                case PrintDialogsStringId.SchedulerSettingsLabelDateFont: return "Tarih yazı tipi";
                case PrintDialogsStringId.SchedulerSettingsLabelAppointmentFont: return "Randevu yazı tipi";
                case PrintDialogsStringId.SchedulerSettingsLabelLayout: return "Yerleşim";
                case PrintDialogsStringId.SchedulerSettingsPrintPageTitle: return "Sayfa başlığını yazdır";
                case PrintDialogsStringId.SchedulerSettingsPrintCalendar: return "Başlığa takvim ekle";
                case PrintDialogsStringId.SchedulerSettingsPrintTimezone: return "Seçili saat dilimini yazdır";
                case PrintDialogsStringId.SchedulerSettingsPrintNotesBlank: return "Not alanı (boş)";
                case PrintDialogsStringId.SchedulerSettingsPrintNotesLined: return "Not alanı (çizgili)";
                case PrintDialogsStringId.SchedulerSettingsNonworkingDays: return "Yalnızca iş günleri";
                case PrintDialogsStringId.SchedulerSettingsExactlyOneMonth: return "Tam olarak bir ay yazdırın";
                case PrintDialogsStringId.SchedulerSettingsLabelWeeksPerPage: return "Sayfa başına hafta";
                case PrintDialogsStringId.SchedulerSettingsNewPageEach: return "Her biri için yeni sayfa başlat";
                case PrintDialogsStringId.SchedulerSettingsStringDay: return "Gün";
                case PrintDialogsStringId.SchedulerSettingsStringMonth: return "Ay";
                case PrintDialogsStringId.SchedulerSettingsStringWeek: return "Hafta";
                case PrintDialogsStringId.SchedulerSettingsStringPage: return "Sayfa";
                case PrintDialogsStringId.SchedulerSettingsStringPages: return "Sayfalar";
                case PrintDialogsStringId.SchedulerSettingsLabelGroupBy: return "Grupla:";
                case PrintDialogsStringId.SchedulerSettingsGroupByNone: return "Hiçbiri";
                case PrintDialogsStringId.SchedulerSettingsGroupByResource: return "Kaynak";
                case PrintDialogsStringId.SchedulerSettingsGroupByDate: return "Tarih";
                case PrintDialogsStringId.GridSettingsLabelPreview: return "Önizleme";
                case PrintDialogsStringId.GridSettingsLabelStyleSettings: return "Stil ayarları";
                case PrintDialogsStringId.GridSettingsLabelFitMode: return "Sayfa sığdırma modu:";
                case PrintDialogsStringId.GridSettingsLabelHeaderCells: return "Üstbilgi hücreleri";
                case PrintDialogsStringId.GridSettingsLabelGroupCells: return "Grup hücreleri";
                case PrintDialogsStringId.GridSettingsLabelDataCells: return "Veri hücreleri";
                case PrintDialogsStringId.GridSettingsLabelSummaryCells: return "Özet hücreleri";
                case PrintDialogsStringId.GridSettingsLabelBackground: return "Arkaplan";
                case PrintDialogsStringId.GridSettingsLabelBorderColor: return "Kenarlık rengi";
                case PrintDialogsStringId.GridSettingsLabelAlternatingRowColor: return "Alternatif satır rengi";
                case PrintDialogsStringId.GridSettingsLabelPadding: return "Dolgu";
                case PrintDialogsStringId.GridSettingsPrintGrouping: return "Grupları yazdır";
                case PrintDialogsStringId.GridSettingsPrintSummaries: return "Özetleri yazdır";
                case PrintDialogsStringId.GridSettingsPrintHierarchy: return "Hiyerarşiyi yazdır";
                case PrintDialogsStringId.GridSettingsPrintHiddenRows: return "Gizli satırları yazdır";
                case PrintDialogsStringId.GridSettingsPrintHiddenColumns: return "Gizli sütunları yazdır";
                case PrintDialogsStringId.GridSettingsPrintHeader: return "Başlıkları her sayfaya yazdır";
                case PrintDialogsStringId.GridSettingsButtonWatermark: return "Fligran...";
                case PrintDialogsStringId.GridSettingsFitPageWidth: return "Genişliğe sığdır";
                case PrintDialogsStringId.GridSettingsNoFit: return "Sığdırma";
                case PrintDialogsStringId.GridSettingsNoFitCentered: return "Sığdırmadan ortala";
                case PrintDialogsStringId.GridSettingsLabelPrint: return "Yazdır";
            }
            return String.Empty;
        }
    }
    public class CustomColorDialogLocalizationProvider : ColorDialogLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                //localizing the static strings
                case ColorDialogStringId.ColorDialogProfessionalTab: return "Basit mod";
                case ColorDialogStringId.ColorDialogWebTab: return "Web";
                case ColorDialogStringId.ColorDialogSystemTab: return "Sistem";
                case ColorDialogStringId.ColorDialogBasicTab: return "Uzman modu";
                case ColorDialogStringId.ColorDialogAddCustomColorButton: return "Özel renklere ekle";
                case ColorDialogStringId.ColorDialogOKButton: return "Tamam";
                case ColorDialogStringId.ColorDialogCancelButton: return "İptal";
                case ColorDialogStringId.ColorDialogNewColorLabel: return "Yeni";
                case ColorDialogStringId.ColorDialogCurrentColorLabel: return "Seçili";
                case ColorDialogStringId.ColorDialogCaption: return "Renk seçimi";
            }
            return base.GetLocalizedString(id);
        }
    }
}
