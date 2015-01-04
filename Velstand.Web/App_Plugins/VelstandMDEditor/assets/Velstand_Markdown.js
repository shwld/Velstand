var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/*
* @class Velstand用Markdownクラス
*/
var VelstandMarkdown = (function (_super) {
    __extends(VelstandMarkdown, _super);
    /*
    * コンストラクタ
    */
    function VelstandMarkdown(item, callbackEvent) {
        if (typeof callbackEvent === "undefined") { callbackEvent = function () {
        }; }
        _super.call(this, item);
        this.addCallback = callbackEvent;
    }
    VelstandMarkdown.prototype.callback = function () {
        this.addCallback();
        this.resumeFocus();
    };

    /*
    * リンクを挿入する
    * @param {String} url 挿入するURL
    * @param {String} title リンクのタイトル
    */
    VelstandMarkdown.prototype.markdownLink = function (url, title) {
        if (typeof title === "undefined") { title = ""; }
        if (title == "") {
            title = this.selection();
        }
        if (this.selection() == "") {
            this.insert("[" + title + "](" + url + " \"" + title + "\")\n");
        } else {
            this.sandwich("[", "](" + url + " \"" + title + "\")\n");
        }
        this.callback();
    };

    /*
    * 画像を挿入する
    * @param {String} image 挿入する画像(dialogService.mediaPickerのcallback)
    * @param {imageHelper} imageHelper UmbracoのimageHelper
    * @param {$scope} $scope angularの$scope
    */
    VelstandMarkdown.prototype.markdownFromMediaPicker = function (image, imageHelper, $scope) {
        // 画像URLを取得
        var imagePropVal = imageHelper.getImagePropertyValue({ imageModel: image, scope: $scope });
        this.insert("\n![" + image.name + "](" + imagePropVal + " \"" + image.name + "\")\n");
        this.callback();
    };

    /*
    * テキストエリアへリンクを挿入する
    * @param {String} link 挿入する画像(dialogService.linkPickerのcallback)
    * @param {contentResource} contentResource UmbracoのcontentResource
    */
    VelstandMarkdown.prototype.markdownFromLinkPicker = function (link, contentResource) {
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
    };

    VelstandMarkdown.prototype.toH2 = function () {
        this.insertHead("\n## ");
        this.callback();
    };

    VelstandMarkdown.prototype.toH3 = function () {
        this.insertHead("\n### ");
        this.callback();
    };

    VelstandMarkdown.prototype.toH4 = function () {
        this.insertHead("\n### ");
        this.callback();
    };

    VelstandMarkdown.prototype.toCode = function () {
        this.sandwich("\n```\n", "\n```\n");
        this.callback();
    };

    VelstandMarkdown.prototype.toNumberList = function () {
        this.sandwichLines("1. ", "");
        this.callback();
    };

    VelstandMarkdown.prototype.toList = function () {
        this.sandwichLines("* ", "");
        this.callback();
    };

    VelstandMarkdown.prototype.toBold = function () {
        this.sandwich(" **", "** ");
        this.callback();
    };

    VelstandMarkdown.prototype.toItaric = function () {
        this.sandwich(" _", "_ ");
        this.callback();
    };

    VelstandMarkdown.prototype.toStrikeout = function () {
        this.sandwich(" ~~", "~~ ");
        this.callback();
    };

    VelstandMarkdown.prototype.insertHr = function () {
        this.insertHead("\n- - -\n\n");
        this.callback();
    };

    VelstandMarkdown.prototype.toBlockquotes = function () {
        this.sandwichLines("> ", "");
        this.callback();
    };
    return VelstandMarkdown;
})(VelstandTextArea);
//# sourceMappingURL=Velstand_Markdown.js.map
