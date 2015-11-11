/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var jshint = require('gulp-jshint');
var del = require('del');
var concat = require('gulp-concat');

var root = "./wwwroot/";

var jsPaths = {
    src: ['./js/**/*.js', "./app/nsbBootstrap.js", "./app/**/*.js"],
    dest: root + "js/"
}

var cssPaths = {
    src: "./css/**/*.css",
    dest: root + "css/"
}

var angularTemplates = {
    src: "./app/**/*.html",
    dest: root
}

gulp.task("clean", function () {
    del(root + '**/*');    // Delete everything in 'wwwroot'
});

gulp.task("lint", function () {
    return gulp.src(jsPaths.src)
      .pipe(jshint())
      .pipe(jshint.reporter("default"));
});

gulp.task("js", function () {
    return gulp.src(jsPaths.src)
        .pipe(concat("app.js"))
        .pipe(gulp.dest(jsPaths.dest));
});

gulp.task("angularTemplates", function () {
    return gulp.src(angularTemplates.src)
        .pipe(gulp.dest(angularTemplates.dest));
});

gulp.task("css", function () {
    return gulp.src(cssPaths.src)
        .pipe(concat("app.css"))
        .pipe(gulp.dest(cssPaths.dest));
});

gulp.task('default', ['clean', 'lint', 'js', 'angularTemplates', 'css'], function () {
    return gulp.src(['index.html', 'data.json', 'favicon.ico'])
        .pipe(gulp.dest(root));
});