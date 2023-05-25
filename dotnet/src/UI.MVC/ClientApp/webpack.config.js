import path from "path";

const __dirname = path.resolve();
import MiniCssExtractPlugin from "mini-css-extract-plugin"

const config = {
    entry: {
        site: './src/js/site.js',
        cssLoader: './src/js/cssLoader.js',
        accountManage: './src/js/accountManage/index.js',
        projectTag: './src/js/projectTag/index.js',
        docReviewWrite: './src/js/docReview/write.js',
        projectModeration: './src/js/projectModeration/index.js',
        moderatorsOverview: './src/js/projectModeration/moderatorsOverview.js',
        markedEmailsOverview: './src/js/projectModeration/markedEmailsOverview.js',
        createProject: './src/js/projectModeration/createProject.js',
        registrationInformationEdit: './src/js/registrationInformationEdit/index.js',
        projectSetting: './src/js/projectSetting/index.js',
        projectDocReview: './src/js/project/docReview.js',
        projectImport: './src/js/project/import.js',
        popup: './src/js/shared/popup.js',
        scroll: './src/js/shared/scroll.js',
        url: './src/js/shared/url.js',
        select: './src/js/shared/select.js',
        bootstrap_js: './src/js/bootstrap_js.js',
        projectsSearch: './src/js/shared/projectsSearch.js',
        validation: './src/js/validation.js',
        dashboard: './src/js/dashboard/dashboard.js',
        projectManage: './src/js/projectManage/index.js',
        analyseComments: './src/js/analyseComments/index.js',
        exportComments: './src/js/analyseComments/exportComments.js',
        docReview: './src/js/docReview/index.js',
        timeline: './src/js/timeline/index.js',
        userProjectPage: './src/js/userProjectPage/project.js',
        projectStyling: './src/js/projectStyling/index.js',
        survey: './src/js/survey/index.js',
        projectManager: './src/js/projectManager/comment.js',
        cookiePopup: './src/js/shared/cookiePopup.js',
    },
    experiments: {
        topLevelAwait: true
    },
    output: {
        filename: '[name].entry.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist')
    },
    devtool: 'source-map',
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [{loader: MiniCssExtractPlugin.loader}, 'css-loader'],
            },
            {
                test: /\.(eot|woff(2)?|ttf|otf|svg)$/i,
                type: 'asset'
            },
            {
                test: /\.s[ac]ss$/i,
                use: [
                    // Creates `style` nodes from JS strings
                    "style-loader",
                    // Translates CSS into CommonJS
                    "css-loader",
                    // Compiles Sass to CSS
                    "sass-loader",
                ],
            },
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        }),
    ]
};
export default config