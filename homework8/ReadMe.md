### HomeWork8
* 选择使用IMGUI和UGUI的方式制作一个血条
* 文件中两个场景即两种方法，加载完成后运行即可使用
* 参考了如下博客资料：[博客1](https://blog.csdn.net/cuiyh1993/article/details/50389327)  [博客2](https://blog.csdn.net/yaoxh6/article/details/80545708)
* 演示视频[在这里](https://www.bilibili.com/video/av24414033/)

-------------------------------

优缺点：
* IMGUI的好处是不需要预制血框和血条，直接用Rect在代码中实现，显得简单明了，缺点是这样做没办法自定义血框血条的外形，比较单一。
* UGUI的好处是可以制作出更加美观的血条，缺点就是制作一个精细的血框还是需要调整一会的，而且在与代码的对接上没那么简洁。
