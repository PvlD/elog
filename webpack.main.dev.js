var path = require("path");

function resolve(filePath) {
  return path.join(__dirname, filePath)
}

module.exports = {

  devtool:  'source-map' ,
  mode: 'development',
  target: 'electron-main',
  output: {
      filename: 'main.js',
      path:  resolve('/dist/main/')
  },
  node: {
      __dirname: false,
      __filename: false,

    },

    entry: resolve("src/main/Main.fs.js"),
    module: {
      rules: [
          {
              test: /\.js$/,
              enforce: "pre",
              use: ["source-map-loader"],
          }
        ]
      }
}
