var markedOptions = (function () {
    function markedOptions() {
    }
    markedOptions.getReMarkedOptions = function () {
        var options = {
            link_list: false,
            h1_setext: false,
            h2_setext: false,
            h_atx_suf: false,
            gfm_code: true,
            trim_code: true,
            li_bullet: "*",
            hr_char: "-",
            indnt_str: "    ",
            bold_char: "*",
            emph_char: "_",
            gfm_del: true,
            gfm_tbls: true,
            tbl_edges: false,
            hash_lnks: false,
            br_only: false,
            col_pre: "col ",
            nbsp_spc: false,
            span_tags: true,
            div_tags: true,
            unsup_tags: {
                // no output
                ignore: "script style noscript",
                // eg: "<tag>some content</tag>"
                inline: "span sup sub i u b center big",
                // eg: "\n\n<tag>\n\tsome content\n</tag>"
                block2: "div form fieldset dl header footer address article aside figure hgroup section",
                // eg: "\n<tag>some content</tag>"
                block1c: "dt dd caption legend figcaption output",
                // eg: "\n\n<tag>some content</tag>"
                block2c: "canvas audio video iframe"
            },
            tag_remap: {
                "i": "em",
                "b": "strong"
            }
        };
        return options;
    };

    /*
    markedOptions.markedRenderer = function () {
        var renderer = new marked.Renderer();
        renderer.image = function (href, title, text) {
            var out = '<img src="/media/velstand/loading.gif" data-original="' + href + '" alt="' + text + '"';
            if (title) {
                out += ' title="' + title + '"';
            }
            out += renderer.options.xhtml ? '/>' : '>';
            return out;
        };
        return renderer;
    };*/
    return markedOptions;
})();

