let gulp = require('gulp');
let iconfont = require('gulp-iconfont');
let iconfontCss = require('gulp-iconfont-css');
let fontName = 'Icons';

gulp.task('iconfont', function(){
   return gulp.src(['assets/icons/*.svg'])
      .pipe(iconfontCss({
        fontName: fontName,
            path: 'assets/css/templates/_favIcons.scss',
            targetPath: './scss/_favicons.scss',
            fontPath: 'projects/sales/src/scss/font/'
      })).pipe(iconfont({
        fontName: fontName,
        formats: ['ttf', 'eot', 'woff', 'woff2', 'svg'],
        normalize: true,
        fontHeight: 1001,
        appendCodepoints: true
      }))
      .pipe(gulp.dest('./scss/font/'));
  });

