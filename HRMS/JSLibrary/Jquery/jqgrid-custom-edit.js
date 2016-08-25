//用于jquery编辑，通用方法，包含grid一般的增加，修改删除方法

//判断grid节点，获取节点
function _getGridByName(gridname) {
    var _gridNode = jQuery("#" + gridname);

    if (_gridNode.length) {
        return _gridNode;
    } else {
        //alert('Grid could not found!');
        return null;
    }
}

//增加
function _gridAdd(gridname) {
    var _gridNode = _getGridByName(gridname);
    if (_gridNode != null) {
        _gridNode.jqGrid("editGridRow", "new", { reloadAfterSubmit: true, closeAfterAdd: true, afterSubmit: _returnServerMsgs });
    }
}

//删除
function _gridDel(gridname) {
    var _gridNode = _getGridByName(gridname);
    if (_gridNode != null) {
        var gr = _gridNode.jqGrid('getGridParam', 'selrow');
        if (gr != null) _gridNode.jqGrid('delGridRow', gr, { reloadAfterSubmit: true, closeAfterDel: true, afterSubmit: _returnServerMsgs });
        else alert("请选择需要删除的记录");
    }
}

//修改
function _gridEdit(gridname) {
    var _gridNode = _getGridByName(gridname);
    if (_gridNode != null) {
        var gr = _gridNode.jqGrid('getGridParam', 'selrow');
        if (gr != null) _gridNode.jqGrid('editGridRow', gr, {
            reloadAfterSubmit: true,
            closeAfterEdit: true,
            afterSubmit: _returnServerMsgs
        });
        else alert("请选择需要修改的记录");
    }
}

//搜索
function _gridSearch(gridname) {
    var _gridNode = _getGridByName(gridname);
    if (_gridNode != null) {
        var tr = $("#gbox_" + gridname + " tr.ui-search-toolbar");
        if (tr.css('display') == 'none')
            tr.show();
        else
            tr.hide();
    }
}

//初始化grid搜索
function _gridSearchInit(gridname) {
    var _gridNode = _getGridByName(gridname);
    if (_gridNode != null) {
        _gridNode.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, defaultSearch: "cn" });
        $('tr[class=ui-search-toolbar]').hide();
    }
}

//Alert the server validation messages
function _returnServerMsgs(response, postdata) {
    var response_txt = response.responseText;
    if (response_txt) {
        var aor = $.parseJSON(response_txt);

        return [aor.success, aor.message, aor.new_id];
    }
}

$(function () {
    //搜索所有的grid初始化搜索
    var _grids = $("div[id^='gbox_']");
    if (_grids.length) {
        $.each(_grids, function () {
            var id = $(this).attr("id");
            if (id) {
                var name = id.replace('gbox_', '');
                _gridSearchInit(name);
            }
        });
    }
    $('tr[class=ui-search-toolbar]').hide();
    //--End
});