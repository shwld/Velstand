/*
 * @class Velstand用Markdownクラス
 */
class VelstandMarkdown extends VelstandTextArea {
    addCallback: any;

    /*
     * コンストラクタ
     */
    constructor(item, callbackEvent = function () { }) {
        super(item);
        this.addCallback = callbackEvent;
    }

    callback() {
        this.addCallback();
        this.resumeFocus();
    }
    
    /*
     * リンクを挿入する
     * @param {String} url 挿入するURL
     * @param {String} title リンクのタイトル
     */
    markdownLink(url, title = "") {
        if (title == "") {
            title = this.selection();
        }
        if (this.selection() == "") {
            this.insert("[" + title + "](" + url + " \"" + title + "\")\n");
        } else {
            this.sandwich("[", "](" + url + " \"" + title + "\")\n");
        }
        this.callback();
    }
    
    /*
     * 画像を挿入する
     * @param {String} image 挿入する画像(dialogService.mediaPickerのcallback)
     * @param {imageHelper} imageHelper UmbracoのimageHelper
     * @param {$scope} $scope angularの$scope
     */
    markdownFromMediaPicker(image, imageHelper, $scope) {
        // 画像URLを取得
        var imagePropVal = imageHelper.getImagePropertyValue({ imageModel: image, scope: $scope });
        this.insert("\n![" + image.name + "](" + imagePropVal + " \"" + image.name + "\")\n");
        this.callback();
    }

    /*
     * テキストエリアへリンクを挿入する
     * @param {String} link 挿入する画像(dialogService.linkPickerのcallback)
     * @param {contentResource} contentResource UmbracoのcontentResource
     */
    markdownFromLinkPicker(link, contentResource) {
        if (link) {
            var href = link.url;

            // content or media
            if (link.id) {
                var niceUrl = contentResource.getNiceUrl(link.id);
                if (niceUrl) {
                    this.markdownLink(href, link.name);
                    this.callback();
                    return true;
                }
            }

            // unlink
            if (!href) {
                return true;
            }

            // is_mail
            if (href.indexOf('@') > 0 && href.indexOf('//') == -1 && href.indexOf('mailto:') == -1) {
                href = 'mailto:' + href;
                this.markdownLink(href, link.name);
                this.callback();
                return true;
            }

            if (/^\s*www\./i.test(href)) {
                href = 'http://' + href;
                this.markdownLink(href, link.name);
                this.callback();
                return true;
            }

            this.markdownLink(href, link.name);
            this.callback();
            return true;
        }
    }

    toH2() {
        this.insertHead("\n## ");
        this.callback();
    }

    toH3() {
        this.insertHead("\n### ");
        this.callback();
    }

    toH4() {
        this.insertHead("\n### ");
        this.callback();
    }

    toCode() {
        this.sandwich("\n```\n", "\n```\n");
        this.callback();
    }

    toNumberList() {
        this.sandwichLines("1. ", "");
        this.callback();
    }

    toList() {
        this.sandwichLines("* ", "");
        this.callback();
    }

    toBold() {
        this.sandwich(" **", "** ");
        this.callback();
    }

    toItaric() {
        this.sandwich(" _", "_ ");
        this.callback();
    }

    toStrikeout() {
        this.sandwich(" ~~", "~~ ");
        this.callback();
    }

    insertHr() {
        this.insertHead("\n- - -\n\n");
        this.callback();
    }

    toBlockquotes() {
        this.sandwichLines("> ", "");
        this.callback();
    }
}
