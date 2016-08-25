//json2
if (!this.JSON2) this.JSON2 = {};
(function() {
    function k(a) { return a < 10 ? "0" + a : a } function n(a) { o.lastIndex = 0; return o.test(a) ? '"' + a.replace(o, function(c) { var d = q[c]; return typeof d === "string" ? d : "\\u" + ("0000" + c.charCodeAt(0).toString(16)).slice(-4) }) + '"' : '"' + a + '"' } function l(a, c) {
        var d, f, i = g, e, b = c[a]; if (b && typeof b === "object" && typeof b.toJSON === "function") b = b.toJSON(a); if (typeof j === "function") b = j.call(c, a, b); switch (typeof b) {
            case "string": return n(b); case "number": return isFinite(b) ? String(b) : "null"; case "boolean": case "null": return String(b); case "object": if (!b) return "null";
                g += m; e = []; if (Object.prototype.toString.apply(b) === "[object Array]") { f = b.length; for (a = 0; a < f; a += 1) e[a] = l(a, b) || "null"; c = e.length === 0 ? "[]" : g ? "[\n" + g + e.join(",\n" + g) + "\n" + i + "]" : "[" + e.join(",") + "]"; g = i; return c } if (j && typeof j === "object") { f = j.length; for (a = 0; a < f; a += 1) { d = j[a]; if (typeof d === "string") if (c = l(d, b)) e.push(n(d) + (g ? ": " : ":") + c) } } else for (d in b) if (Object.hasOwnProperty.call(b, d)) if (c = l(d, b)) e.push(n(d) + (g ? ": " : ":") + c); c = e.length === 0 ? "{}" : g ? "{\n" + g + e.join(",\n" + g) + "\n" + i + "}" : "{" + e.join(",") + "}";
                g = i; return c
        }
    } if (typeof Date.prototype.toJSON !== "function") { Date.prototype.toJSON = function() { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + k(this.getUTCMonth() + 1) + "-" + k(this.getUTCDate()) + "T" + k(this.getUTCHours()) + ":" + k(this.getUTCMinutes()) + ":" + k(this.getUTCSeconds()) + "Z" : null }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function() { return this.valueOf() } } var p = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
o = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, g, m, q = { "\u0008": "\\b", "\t": "\\t", "\n": "\\n", "\u000c": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, j; if (typeof JSON2.stringify !== "function") JSON2.stringify = function(a, c, d) {
    var f; m = g = ""; if (typeof d === "number") for (f = 0; f < d; f += 1) m += " "; else if (typeof d === "string") m = d; if ((j = c) && typeof c !== "function" && (typeof c !== "object" || typeof c.length !== "number")) throw new Error("JSON.stringify"); return l("",
{ "": a })
}; if (typeof JSON2.parse !== "function") JSON2.parse = function(a, c) {
    function d(f, i) { var e, b, h = f[i]; if (h && typeof h === "object") for (e in h) if (Object.hasOwnProperty.call(h, e)) { b = d(h, e); if (b !== undefined) h[e] = b; else delete h[e] } return c.call(f, i, h) } a = String(a); p.lastIndex = 0; if (p.test(a)) a = a.replace(p, function(f) { return "\\u" + ("0000" + f.charCodeAt(0).toString(16)).slice(-4) }); if (/^[\],:{}\s]*$/.test(a.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,
"]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) { a = eval("(" + a + ")"); return typeof c === "function" ? d({ "": a }, "") : a } throw new SyntaxError("JSON.parse");
}
})();


(function(a) {
    a.jgrid =
 { defaults:
  { recordtext: "记录数: {0} - {1} / {2}", emptyrecords: "没有数据", loadtext: "读取中...", pgtext: "页数: {0} / {1}" }, search: { caption: "查找中...", Find: "查找", Reset: "重置",
      odata: ["equal", "not equal", "less", "less or equal", "greater", "greater or equal", "begins with", "does not begin with", "is in", "is not in", "ends with", "does not end with", "contains", "does not contain"], groupOps: [{ op: "AND", text: "all" }, { op: "OR", text: "any"}],
      matchText: " match", rulesText: " rules"
  }, edit: { addCaption: "新增", editCaption: "编辑", bSubmit: "提交", bCancel: "取消", bClose: "关闭", saveData: "是否保存所做的修改?", bYes: "是", bNo: "否",
      bExit: "取消", msg: { required: "请填写", number: "请输入数字", minValue: "数值不能小于", maxValue: "数值不能大于", email: "不是正确的Email", integer: "请输入整数", date: "请输入合法的数据", url: "不是正确的网址，请以('http://' or 'https://')作为开头", minLength: "输入的长度须大于等于", maxLength: "输入的长度须小于等于" }
  },
     view: { caption: "查看数据", bClose: "关闭" }, del: { caption: "删除", msg: "删除所选数据?", bSubmit: "删除", bCancel: "取消" },
     nav: { edittext: "改", edittitle: "编辑所选数据", addtext: "增", addtitle: "新增", deltext: "删", deltitle: "删除所选数据", searchtext: "", searchtitle: "查找数据", refreshtext: "刷新", refreshtitle: "刷新数据", alertcap: "警告", alerttext: "请选择数据行", viewtext: "", viewtitle: "查看所选行" }, col: { caption: "Show/Hide Columns", bSubmit: "Submit", bCancel: "Cancel" }, errors: { errcap: "Error", nourl: "No url is set", norecords: "No records to process", model: "Length of colNames <> colModel!" }, formatter: { integer: { thousandsSeparator: " ", defaultValue: "0" }, number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: "0.00" }, currency: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: "0.00" }, date: { dayNames: ["Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], monthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], AmPm: ["am", "pm", "AM", "PM"], S: function(b) { return b < 11 || b > 13 ? ["st", "nd", "rd", "th"][Math.min((b - 1) % 10, 3)] : "th" }, srcformat: "Y-m-d", newformat: "d/m/Y", masks: { ISO8601Long: "Y-m-d H:i:s", ISO8601Short: "Y-m-d", ShortDate: "n/j/Y", LongDate: "l, F d, Y", FullDateTime: "l, F d, Y g:i:s A", MonthDay: "F d", ShortTime: "g:i A", LongTime: "g:i:s A", SortableDateTime: "Y-m-d\\TH:i:s", UniversalSortableDateTime: "Y-m-d H:i:sO", YearMonth: "F, Y" }, reformatAfterEdit: false }, baseLinkUrl: "", showAction: "", target: "", checkbox: { disabled: true }, idName: "id" }
 }
})(jQuery);

function validateDuplicatedValue(grid, value, colText, colName, exception) {
    var success = [true, ""];
    var fail = [false, colText + "内容不能重复"];
    var notEmpty = [false, colText + "不能为空"];

    if (value == null || value == "undefined" || value == "")
        return notEmpty;

    // 标明可以跳过的内容   
    for (var i = 0; i < exception.length; i++) {
        if (value.indexOf(exception[i]) > -1)
            return success;
    }    

    var ids = grid.getDataIDs();
    var selectedId = grid.getGridParam('selarrrow');

    var found = false;
    for (var i = 0; i < ids.length; i++) {
        if (selectedId == ids[i])
            continue;

        var data = grid.getRowData(ids[i]);

        if (value == data[colName]) {
            found = true;
            break;
        }
    }

    return found == true ? fail : success;
}

function afterload(data) {
    //var data = $.jgrid.parse(request.responseText);
    AddSummaryRow($(this).context.id, data);
    LoadComplete($(this).context.id, data);
}
function AddSummaryRow(gridID, data) {
    if (data.footer) {
        //$(ts).addRowData('sum', data.footerData, "last", 0);
        $('#' + gridID).footerData('set', eval(data).footer, false);
        //$(".footrow").css({ fontWeight: 'bolder', background: '#ffccff' });
    }
}
function LoadComplete(gridID, data) {
    $("#sum td").css({ fontWeight: 'bolder', background: '#ffccff' });
    //$(".ui-jqgrid-btable tr:odd").addClass("oddRow");
    //$(".ui-jqgrid-btable tr:even").addClass("evenRow");
    var editable = false;
    var cols = $('#' + gridID).getCol('Edit');
    if (cols.length > 0) {
        editable = true;
    }

    if (editable) {
        $("#" + gridID + " #sum td:last-child").text("合计");
        var ids = $('#' + gridID).getDataIDs();
        for (var i = 0; i < ids.length; i++) {
            var cl = ids[i];
            if (cl === "sum") {
                return;
            }
            var ce = $('#' + gridID).jqGrid().getGridParam('customerEdit');
            if (ce != null) {
                ce(sender, i);
            }
            else {
                e = "<input style='height:22px;width:40px;' type='button' class='add' value='编辑' onclick=EnterEdit('" + gridID + "'," + cl + "); />";
                $('#' + gridID).setRowData(ids[i], { Edit: e });
            }
        }
    }
}

function EnterEdit(gridID, row) {
    if (!row) {
        row = "new"; //Add mode
    }
    jQuery('#' + gridID).editGridRow(row, {
        closeAfterEdit: true,
        closeAfterAdd: true,
        addCaption: "添加",
        modal: true,
        processData: "处理中...",
        afterComplete: AfterEditComplete
    });
}

function defaultAfterSubmit(response, postdata) {
    var aor = JSON.parse(response.responseText);
    if (aor.success)
    {
        if (aor.message) {
            alert(aor.message)
        }
        else {
            alert('数据提交成功');
        }
    }
    else if (aor.success == false) {
        ReportError(aor.message);        
    }
    return [aor.success, aor.message, aor.new_id]
}
function AfterEditComplete(response, postdata, formid) {
    var aor = JSON.parse(response.responseText);
    alert(aor.message);
    if (aor.success) {
        if (aor.message) {
            alert(aor.message)
        }
        return true;
    }
    else {
        ReportError(aor.message);
        return false;
    }
}

function gridLoadError(xhr, status, error) {
    alert("获取数据出错，请联系管理员，错误信息:" + status);
}
function GridErrorHanlder(ex) {

}

function exportGridContents(gridId, url) {
    grid = $("#" + gridId);
    var rowIds = grid.getDataIDs();
    var rows = new Array();
    var rowVisible = new Array();
    
    var colNames = grid.getGridParam('colNames');
    var gridModels = grid.getGridParam('colModel');
    var gridCaption = grid.getGridParam("caption");
    
    for (var i = 0; i < gridModels.length; i++) {
        rowVisible.push(gridModels[i].hidden);
    }

    for (var i = 0; i < rowIds.length; i++) {
        var data = grid.getRowData(rowIds[i]);
        rows.push(data);
    }

    var gdata = JSON2.stringify(rows);
    var vdata = JSON2.stringify(rowVisible);
    //var hdata = JSON2.stringify(colNames);

    var final_hdata_array = new Array();
    for (var i = 0; i < colNames.length; i++) {
        if (colNames[i].indexOf("<input") == -1)
            final_hdata_array.push(colNames[i]);
    }
    var hdata = JSON2.stringify(final_hdata_array);

    $.post(url, { CommonGridOp: 'export2excel', griddata: gdata, visibles: vdata, headers: hdata, caption: gridCaption, ts: jqGrid_GetTimeStamp() }, function(data) {
        if (data == 'false' || data == '')
            alert("导出失败。可能原因：数据为空");
        else
            window.open(url + "?CommonGridOp=printexcel&filepath=" + data + "&filename=" + encodeURI(gridCaption));        
    });
}

function hideGrid(gridId) {
    $('#gbox_' + gridId + ' .HeaderButton').click();    
    //exportGridContents(gridId, "SearchResultHandler.ashx");
}
/*
function postSuccess(response) {
if (response.responseText == "success") {
return true;
}
else {
ReportError("保存出错，" + response.responseText);
return false;
}
}
function Aftersavefunc(id, responseText) {
lastPostEditor.value = "编辑";
lastPostEditor = null;
}

function OnEditError(id, responseText) {
ReportError("保存出错,请求失败！");
}
*/

function jqGrid_GetTimeStamp() {
    if ($.browser.msie)
        return Date.parse(new Date());
    else
        return "0";
}

/* Three function for kiosk projects, but maybe it will be useful for others */
function doRemoveDelDialog(data, postedValues, jqGridId) {
    var dialogId = 'delmod' + jqGridId;    

    if ($("#" + dialogId).html() != null) {
        $("div.jqmOverlay").remove();
        $("#" + dialogId).remove();
    }


    return true;
}

function doBatchRestore(jqGridId, OnRestoreComplete) {
    var gridInstance = $('#' + jqGridId);
    var selected = gridInstance.getGridParam('selarrrow');
    if (selected == null || selected.length == 0) {
        alert("请选择要恢复的记录！");
        return false;
    }

    var ids = JSON2.stringify(selected);
    gridInstance.delGridRow("restore", { delData: { restore_ids: ids }, recreateForm: true, reloadAfterSubmit: true, msg: '确定要还原选中的记录？', caption: '还原', modal: true, bSubmit: '还原', afterSubmit: function (data, postedValues) { var ret = doRemoveDelDialog(data, postedValues, jqGridId); if (OnRestoreComplete) OnRestoreComplete.call(); return ret; } });

    return true;
}

function doBatchDelete(jqGridId, OnDeleteComplete) {
    var gridInstance = $('#' + jqGridId);
    var selected = gridInstance.getGridParam('selarrrow');
    if (selected == null || selected.length == 0) {
        alert("请选择要删除的记录！");
        return false;
    }

    var ids = JSON2.stringify(selected);
    gridInstance.delGridRow("delete", { delData: { del_ids: ids }, width: 400, reloadAfterSubmit: true, msg: '确定要删除选中的记录，状态为已删除的数据将被彻底删除？', caption: '删除', modal: true, afterSubmit: function (data, postedValues) { var ret = doRemoveDelDialog(data, postedValues, jqGridId); if (OnDeleteComplete) OnDeleteComplete.call(); return ret; } });

    return true;
}  

