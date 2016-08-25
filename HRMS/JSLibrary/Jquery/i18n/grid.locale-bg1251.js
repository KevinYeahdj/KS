;(function($){
/**
 * jqGrid Bulgarian Translation 
 * Tony Tomov tony@trirand.com
 * http://trirand.com/blog/ 
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
**/
$.jgrid = {
	defaults : {
		recordtext: "{0} - {1} от {2}",
		emptyrecords: "?ма запи??",
		loadtext: "Зареждам...",
		pgtext : "Ст? {0} от {1}"
	},
	search : {
		caption: "Търсен?..",
		Find: "Намери",
		Reset: "Изчист?,
		odata : ['равн?, 'различно', 'по-малк?, 'по-малк?ил?','по-го?мо','по-го?мо ил?=', 'започв??,'не започв??,'се намира ?,'не се намира ?,'завършва ?,'не завършав??,'съдърж?, 'не съдърж? ],
	    groupOps: [	{ op: "AND", text: " ?" },	{ op: "OR",  text: "ИЛ? }	],
		matchText: " включи",
		rulesText: " клауза"
	},
	edit : {
		addCaption: "Но?Запи?,
		editCaption: "Редакц? Запи?,
		bSubmit: "Запиши",
		bCancel: "Изхо?,
		bClose: "Затвор?,
		saveData: "Даннит?са променен? Да съхраня ли променит?",
		bYes : "Да",
		bNo : "Не",
		bExit : "Отка?,
		msg: {
		    required:"Полето ?задължително",
		    number:"Въведете валидн?числ?",
		    minValue:"стойността трябв?да ?по-го?ма ил?равн?от",
		    maxValue:"стойността трябв?да ?по-малк?ил?равн?от",
		    email: "не ?валиде?ел. адре?,
		    integer: "Въведете валидн??ло числ?,
			date: "Въведете валидн?дата",
			url: "e невалиде?URL. Изискава се префик?'http://' ил?'https://')",
			nodefined : " ?недефинирана!",
			novalue : " изискв?връщан?на стойност!",
			customarray : "Потреб. Функция трябв?да върн?маси?",
			customfcheck : "Потребителск?функция ?задължителна пр?този ти?елемен?"
		}
	},
	view : {
	    caption: "Прегле?запи?,
	    bClose: "Затвор?
	},
	del : {
		caption: "Изтриван?,
		msg: "Да изтр? ли избран??запи?",
		bSubmit: "Изтрий",
		bCancel: "Отка?
	},
	nav : {
		edittext: " ",
		edittitle: "Редакц? избран запи?,
		addtext:" ",
		addtitle: "Доба?не но?запи?,
		deltext: " ",
		deltitle: "Изтриван?избран запи?,
		searchtext: " ",
		searchtitle: "Търсен?запи??",
		refreshtext: "",
		refreshtitle: "Обнови таблиц?,
		alertcap: "Предупреждение",
		alerttext: "Мо?, изберете запи?,
		viewtext: "",
		viewtitle: "Прегле?избран запи?
	},
	col : {
		caption: "Избо?колони",
		bSubmit: "Ок",
		bCancel: "Изхо?	
	},
	errors : {
		errcap : "Грешка",
		nourl : "?ма посоче?url адре?,
		norecords: "?ма запи?за обработк?,
		model : "Модела не съответств?на именат?"	
	},
	formatter : {
		integer : {thousandsSeparator: " ", defaultValue: '0'},
		number : {decimalSeparator:".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00'},
		currency : {decimalSeparator:".", thousandsSeparator: " ", decimalPlaces: 2, prefix: "", suffix:" лв.", defaultValue: '0.00'},
		date : {
			dayNames:   [
				"Не?, "По?, "Вт", "Ср", "Че?, "Пе?, "Съ?,
				"Неде?", "Понеделник", "Вторни?, "Сряда", "Четвъртъ?, "Петъ?, "Събота"
			],
			monthNames: [
				"Ян?, "Фе?, "Ма?, "Ап?, "Ма?, "Юн?, "Юл?, "Ав?, "Се?, "Ок?, "Но?, "Де?,
				"Януари", "Февруари", "Март", "Апри?, "Ма?, "Юн?, "Юл?, "Август", "Септемвр?, "Октомври", "Ноемвр?, "Декември"
			],
			AmPm : ["","","",""],
			S: function (j) {
				if(j==7 || j==8 || j== 27 || j== 28) {
					return 'ми';
				}
				return ['ви', 'ри', 'ти'][Math.min((j - 1) % 10, 2)];
			},
			srcformat: 'Y-m-d',
			newformat: 'd/m/Y',
			masks : {
		        ISO8601Long:"Y-m-d H:i:s",
		        ISO8601Short:"Y-m-d",
		        ShortDate: "n/j/Y",
		        LongDate: "l, F d, Y",
		        FullDateTime: "l, F d, Y g:i:s A",
		        MonthDay: "F d",
		        ShortTime: "g:i A",
		        LongTime: "g:i:s A",
		        SortableDateTime: "Y-m-d\\TH:i:s",
		        UniversalSortableDateTime: "Y-m-d H:i:sO",
		        YearMonth: "F, Y"
		    },
		    reformatAfterEdit : false
		},
		baseLinkUrl: '',
		showAction: '',
		target: '',
		checkbox : {disabled:true},
		idName : 'id'
	}
};
})(jQuery);
