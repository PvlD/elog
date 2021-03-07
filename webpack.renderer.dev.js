

var path = require("path");
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');
const { spawn, execSync } = require('child_process');
const CopyWebpackPlugin = require('copy-webpack-plugin');



const  renderName ="renderer"

const distDir = './dist/' + renderName +  '/'

const projDir = "./src/" + renderName + "/"

const staticDir = projDir + "static/"





const  scssPath = projDir + "scss/main.scss"


function resolve(filePath) {
  return path.join(__dirname, filePath)
}

const port = process.env.PORT || 1212;
const publicPath = `http://localhost:${port}/dist/`+ renderName;


const no_main =  process.env.NOMAIN || false

console.log("no_main:",no_main)


module.exports = {


  devtool:  'source-map' ,
  mode: 'development',
  target: 'electron-renderer',
  output: {
      filename: 'main.js',
      path:  resolve(distDir),
      publicPath:publicPath
  },


  node: {
      __dirname: false,
      __filename: false,

    },
    resolve: {
      extensions: ['.js', '.jsx', '.json', '.ts', '.css','.scss'],
      modules: [path.join(__dirname, '../src'), 'node_modules'],
    },


  entry: [resolve(  projDir + "Renderer.fs.js"  )],
  module: {
    rules: [

      {
        test: /\.(scss|css)$/,
        use: [


            {
              loader: 'style-loader'
          },
          {
            loader: 'css-loader',
            options: {
              sourceMap:true
              }

        },
        {
          loader: 'sass-loader',
          options: {
            sourceMap:true
            }

      },

        ],
    },
    {
      test: /\.(jpg|png|svg|ico|icns)$/,
      loader: 'file-loader',
      options: {
          name: '[path][name].[ext]',
      },
  },
  {
      test: /\.(eot|ttf|woff|woff2)$/,
      loader: 'file-loader',
      options: {
          name: 'fonts/[name].[ext]',
      },
  },
  {
    test: /\.js$/,
    enforce: "pre",
    use: ["source-map-loader"],
  }

    ]
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: 'styles.css',
    }),
    new HtmlWebpackPlugin({
      template: path.resolve(__dirname, projDir + 'index.html'),
      filename: resolve(distDir + 'index.html'),


  }),

  new CopyWebpackPlugin({
    patterns: [
        { from: path.resolve(__dirname,  staticDir)
        }
    ]
})
,
  new webpack.NoEmitOnErrorsPlugin(),


  new webpack.EnvironmentPlugin({
    NODE_ENV: 'development',
  }),

  new webpack.LoaderOptionsPlugin({
    debug: true,
  }),



  ],

  devServer: {
    port,
    publicPath,
    compress: true,
    noInfo: false,
    stats: 'errors-only',
    inline: true,
    lazy: false,
    hot: true,
    headers: { 'Access-Control-Allow-Origin': '*' },
    contentBase: [path.join(__dirname, 'dist/' + renderName),path.join(__dirname,"static") ],
    watchOptions: {
      aggregateTimeout: 300,
      ignored: /node_modules/,
      poll: 100,
    },
    historyApiFallback: {
      verbose: true,
      disableDotRule: false,
    },
    before() {

      if (!no_main )
      {
      console.log('Starting Main Process...');
        spawn('npm', ['run', 'start:main'], {
          shell: true,
          env: process.env,
          stdio: 'inherit',
        })
          .on('close', (code) => process.exit(code))
          .on('error', (spawnError) => console.error(spawnError));
      }
    },
  },

}
