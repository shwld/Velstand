class VelstandTextAreaModel {
    value: string = "";
    selectionStart: number = 0;
    selectionEnd: number = 0;
    setSelectionRange: any;
    focus: any;
}

/*
 * @class テキストエリアの文字列を操作するクラス
 */
class VelstandTextArea {
    textArea: VelstandTextAreaModel = new VelstandTextAreaModel();
    undoList: string[] = new Array();
    redoList: string[] = new Array();
    beforeUpdate: string = "";
    caret: number = 0;

    /*
     * コンストラクタ
     */
    constructor(val = "") {
        this.beforeUpdate = val;
        this.textArea.value = val;
    }

    resumeFocus() {
        this.textArea.setSelectionRange(this.caret, this.caret);
        this.textArea.focus();
    }

    updateTextArea(item) {
        this.textArea = item;
        if (this.isUpdate()) {
            this.undoList.push(this.beforeUpdate);
            if (this.undoList.length > 25) {
                this.undoList.shift();
            }
        }
        this.beforeUpdate = this.textArea.value;
    }

    isUpdate(): boolean {
        if (this.textArea.value != this.beforeUpdate) {
            return true;
        }
        return false;
    }

    getValue() {
        return this.textArea.value;
    }

    setValue(text, setCalet: number = -1) {
        this.caret = setCalet == -1 ? this.textArea.selectionStart : setCalet;
        this.textArea.value = text;
        this.updateTextArea(this.textArea);
    }

    undo() {
        if (this.undoList.length != 0) {
            this.caret =this.textArea.selectionStart;
            var temp = this.undoList.pop();
            this.redoList.push(this.textArea.value);
            this.textArea.value = temp;
        }
        this.resumeFocus();
    }

    redo() {
        if (this.redoList.length != 0) {
            this.caret = this.textArea.selectionStart;
            var temp = this.redoList.pop();
            this.undoList.push(this.textArea.value);
            this.textArea.value = temp;
        }
        this.resumeFocus();
    }

    /*
     * 選択中の文字列の先頭を基準に前と後ろの文字を取得
     */
    prefix() {
        var result = new Array();
        result[0] = this.textArea.value.substring(0, this.textArea.selectionStart);
        result[1] = this.textArea.value.substring(this.textArea.selectionStart);
        return result;
    }

    /*
     * 選択中の文字列を取得する
     */
    selection() {
        return this.textArea.value.substring(this.textArea.selectionStart, this.textArea.selectionEnd);
    }

    /*
     * 選択中の文字列の末尾を基準に前と後ろの文字を取得
     */
    suffix() {
        var result = new Array();
        result[0] = this.textArea.value.substring(0, this.textArea.selectionEnd);
        result[1] = this.textArea.value.substring(this.textArea.selectionEnd);
        return result;
    }

    /*
     * 改行文字の位置を基準に前後の文字列を返す
     * @param {String} textString 挿入するテキスト
     */
    indexOfBreak(textString: string) {
        var result = new Array();
        textString = textString.replace(/\r\n|\r/g, "\n"); 
        var index: number = textString.indexOf("\n");
        if (index > -1) {
            result[0] = textString.substring(0, index);
            result[1] = textString.substring(index);
            return result;
        }
        result[0] = "";
        result[1] = textString;
        return result;
    }
    
    /*
     * 改行文字の位置を基準に前後の文字列を返す(末尾から検索)
     * @param {String} textString 挿入するテキスト
     */
    lastIndexOfBreak(textString: string) {
        var result = new Array();
        textString = textString.replace(/\r\n|\r/g, "\n"); 
        var index: number = textString.lastIndexOf("\n");
        if (index > -1) {
            result[0] = textString.substring(0, index + 1);
            result[1] = textString.substring(index + 1);
            return result;
        }
        result[0] = textString;
        result[1] = "";
        return result;
    }

    /*
     * 文字列をキャレットの箇所へ挿入する
     * @param {String} textString 挿入するテキスト
     */
    insert(textString: string) {
        this.setValue(this.prefix()[0] + textString + this.prefix()[1]);
    }
    
    /*
     * 文字列を編集中の行の行頭へ挿入する
     * @param {String} textString 挿入するテキスト
     */
    insertHead(textString: string) {
        var prefix = this.lastIndexOfBreak(this.prefix()[0]);
        var setCalet = this.textArea.selectionStart + textString.length;
        this.setValue(prefix[0] + textString + prefix[1] + this.prefix()[1], setCalet);
    }

    /*
     * 選択中の文字列を挟み込むように文字を挿入する
     * @param {String} prefixString 挿入するテキスト(先頭)
     * @param {String} suffixString 挿入するテキスト(末尾)
     */
    sandwich(prefixString: string, suffixString: string) {
        var setCalet = this.textArea.selectionStart + prefixString.length;
        this.setValue(this.prefix()[0] + prefixString + this.selection() + suffixString + this.suffix()[1], setCalet);
    }

    /*
     * 選択中の文字列を改行で区切り、各行の先頭、末尾に文字を挿入する
     * @param {String} textString 元となるテキスト
     * @param {String} prefixString 挿入するテキスト
     * @param {String} suffixString 挿入するテキスト
     */
    sandwichLines(prefixString: string, suffixString: string) {
        var result: string = "\n";
        var textString = this.selection().replace(/\r\n|\r/g, "\n");
        var lines = textString.split('\n');
        for (var i = 0; i < lines.length; i++) {
            if (lines[i] == "") {
                continue;
            }
            result += prefixString + lines[i] + suffixString + "\n";
        }
        this.setValue(this.prefix()[0] + result + this.suffix()[1]);
    }
}
 