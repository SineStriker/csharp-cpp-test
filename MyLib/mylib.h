#pragma once

#ifndef MYLIB_API
#ifdef MYLIB_STATIC
#define MYLIB_API
#else
#ifdef MYLIB_LIBRARY
#define MYLIB_API __declspec(dllexport)
#else
#define MYLIB_API __declspec(dllimport)
#endif
#endif
#endif

struct Foo {
  int a;
};

struct Bar {
  int val;
  Foo *foo;
};

extern "C" {

MYLIB_API Bar *newBar(int a, int b);

MYLIB_API int getFooProp(Bar *bar);

}