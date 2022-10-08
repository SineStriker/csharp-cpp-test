#include "mylib.h"

#include <stdint.h>
#include <stdio.h>


Bar *newBar(int a, int b) {
  auto foo = new Foo();
  foo->a = a;

  auto bar = new Bar();
  bar->val = b;
  bar->foo = foo;

  printf("[C] newBar called with %d, %d\n", a, b);
  return bar;
}

int getFooProp(Bar *bar) {
  printf("[C] getFooProp called with %lld\n", (int64_t)bar);
  return bar->foo->a;
}