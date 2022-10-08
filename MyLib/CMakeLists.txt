cmake_minimum_required(VERSION 3.17)

project(MyLib
    VERSION 0.0.0.1
    LANGUAGES CXX
)

set(MYLIB_BUILD_STATIC off)

set(MYLIB_SOURCES mylib.h mylib.cpp)

if (MYLIB_BUILD_STATIC)
    add_library(${PROJECT_NAME} STATIC ${MYLIB_SOURCES})
    target_compile_definitions(${PROJECT_NAME} PUBLIC MYLIB_BUILD_STATIC)
else()
    add_library(${PROJECT_NAME} SHARED ${MYLIB_SOURCES})
endif()

target_compile_definitions(${PROJECT_NAME} PRIVATE MYLIB_LIBRARY)
