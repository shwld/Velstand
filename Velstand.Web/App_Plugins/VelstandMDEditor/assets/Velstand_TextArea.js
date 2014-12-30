var VelstandTextAreaModel = (function () {
    function VelstandTextAreaModel() {
        this.value = "";
        this.selectionStart = 0;
        this.selectionEnd = 0;
    }
    return VelstandTextAreaModel;
})();

/*
* @class テキストエリアの文字列を操作するクラス
*/
var VelstandTextArea = (function () {
    /*
    * コンストラクタ
    */
    function VelstandTextArea(val) {
        if (typeof val === "undefined") { val = ""; }
        this.textArea = new VelstandTextAreaModel();
        this.undoList = new Array();
        this.redoList = new Array();
        this.beforeUpdate = "";
        this.caret = 0;
        this.beforeUpdate = val;
        this.textArea.value = val;
    }
    VelstandTextArea.prototype.resumeFocus = function () {
        this.textArea.setSelectionRange(this.caret, this.caret);
        this.textArea.focus();
    };

    VelstandTextArea.prototype.updateTextArea = function (item) {
        this.textArea = item;
        if (this.isUpdate()) {
            this.undoList.push(this.beforeUpdate);
            if (this.undoList.length > 25) {
                this.undoList.shift();
            }
        }
        this.beforeUpdate = this.textArea.value;
    };

    VelstandTextArea.prototype.isUpdate = function () {
        if (this.textArea.value != this.beforeUpdate) {
            return true;
        }
        return false;
    };

    VelstandTextArea.prototype.getValue = function () {
        return this.textArea.value;
    };

    VelstandTextArea.prototype.setValue = function (text, setCalet) {
        if (typeof setCalet === "undefined") { setCalet = -1; }
        this.caret = setCalet == -1 ? this.textArea.selectionStart : setCalet;
        this.textArea.value = text;
        this.updateTextArea(this.textArea);
    };

    VelstandTextArea.prototype.undo = function () {
        if (this.undoList.length != 0) {
            this.caret = this.textArea.selectionStart;
            var temp = this.undoList.pop();
            this.redoList.push(this.textArea.value);
            this.textArea.value = temp;
        }
        this.resumeFocus();
    };

    VelstandTextArea.prototype.redo = function () {
        if (this.redoList.length != 0) {
            this.caret = this.textArea.selectionStart;
            var temp = this.redoList.pop();
            this.undoList.push(this.textArea.value);
            this.textArea.value = temp;
        }
        this.resumeFocus();
    };

    /*
    * 選択中の文字列の先頭を基準に前と後ろの文字を取得
    */
    VelstandTextArea.prototype.prefix = function () {
        var result = new Array();
        result[0] = this.textArea.value.substring(0, this.textArea.selectionStart);
        result[1] = this.textArea.value.substring(this.textArea.selectionStart);
        return result;
    };

    /*
    * 選択中の文字列を取得する
    */
    VelstandTextArea.prototype.selection = function () {
        return this.textArea.value.substring(this.textArea.selectionStart, this.textArea.selectionEnd);
    };

    /*
    * 選択中の文字列の末尾を基準に前と後ろの文字を取得
    */
    VelstandTextArea.prototype.suffix = function () {
        var result = new Array();
        result[0] = this.textArea.value.substring(0, this.textArea.selectionEnd);
        result[1] = this.textArea.value.substring(this.textArea.selectionEnd);
        return result;
    };

    /*
    * 改行文字の位置を基準に前後の文字列を返す
    * @param {String} textString 挿入するテキスト
    */
    VelstandTextArea.prototype.indexOfBreak = function (textString) {
        var result = new Array();
        textString = textString.replace(/\r\n|\r/g, "\n");
        var index = textString.indexOf("\n");
        if (index > -1) {
            result[0] = textString.substring(0, index);
            result[1] = textString.substring(index);
            return result;
        }
        result[0] = "";
        result[1] = textString;
        return result;
    };

    /*
    * 改行文字の位置を基準に前後の文字列を返す(末尾から検索)
    * @param {String} textString 挿入するテキスト
    */
    VelstandTextArea.prototype.lastIndexOfBreak = function (textString) {
        var result = new Array();
        textString = textString.replace(/\r\n|\r/g, "\n");
        var index = textString.lastIndexOf("\n");
        if (index > -1) {
            result[0] = textString.substring(0, index + 1);
            result[1] = textString.substring(index + 1);
            return result;
        }
        result[0] = textString;
        result[1] = "";
        return result;
    };

    /*
    * 文字列をキャレットの箇所へ挿入する
    * @param {String} textString 挿入するテキスト
    */
    VelstandTextArea.prototype.insert = function (textString) {
        var setCalet = this.textArea.selectionStart + textString.length;
        this.setValue(this.prefix()[0] + textString + this.prefix()[1], setCalet);
    };

    /*
    * 文字列を編集中の行の行頭へ挿入する
    * @param {String} textString 挿入するテキスト
    */
    VelstandTextArea.prototype.insertHead = function (textString) {
        var prefix = this.lastIndexOfBreak(this.prefix()[0]);
        var setCalet = this.textArea.selectionStart + textString.length;
        this.setValue(prefix[0] + textString + prefix[1] + this.prefix()[1], setCalet);
    };

    /*
    * 選択中の文字列を挟み込むように文字を挿入する
    * @param {String} prefixString 挿入するテキスト(先頭)
    * @param {String} suffixString 挿入するテキスト(末尾)
    */
    VelstandTextArea.prototype.sandwich = function (prefixString, suffixString) {
        var setCalet = this.textArea.selectionStart + prefixString.length;
        this.setValue(this.prefix()[0] + prefixString + this.selection() + suffixString + this.suffix()[1], setCalet);
    };

    /*
    * 選択中の文字列を改行で区切り、各行の先頭、末尾に文字を挿入する
    * @param {String} textString 元となるテキスト
    * @param {String} prefixString 挿入するテキスト
    * @param {String} suffixString 挿入するテキスト
    */
    VelstandTextArea.prototype.sandwichLines = function (prefixString, suffixString) {
        var result = "\n";
        var textString = this.selection().replace(/\r\n|\r/g, "\n");
        var lines = textString.split('\n');
        for (var i = 0; i < lines.length; i++) {
            if (lines[i] == "") {
                continue;
            }
            result += prefixString + lines[i] + suffixString + "\n";
        }
        var setCalet = this.textArea.selectionStart + result.length;
        this.setValue(this.prefix()[0] + result + this.suffix()[1], setCalet);
    };
    return VelstandTextArea;
})();
//# sourceMappingURL=Velstand_TextArea.js.map
