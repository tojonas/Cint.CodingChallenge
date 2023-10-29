const path = require('path');

module.exports = {
    entry: {
        site: './src/js/app.js'
    },
    devtool: 'inline-source-map',
    output: {
        filename: 'site.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist'),
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader']
            }
        ]
    }
};
