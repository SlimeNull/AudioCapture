<div align="center">

  
# AudioCapture


![Cover](/Assets/cover.png)

  
</div>


<br/>

这是一个可以捕获音频输入输出数据, 对齐进行傅里叶变换以分析频谱的程序. 可以指定采样率, 采样数量等参数. 支持手动单次采集, 也支持自动连续采集.

## 用到的东西

使用 Windows 的音频接口 WASAPI 进行捕获, 在 C# 上, 有已经封装好的库, 可以直接调用 NAudio 来方便的进行使用.

要进行音频的频域分析, 需要进行傅里叶变换, 这里使用的开源的库 FftSharp, 直接对频域数据进行傅里叶变换并保存.

封装一个 “可视化器” 用于方便的进行可视化操作, 当捕获器得到数据的时候, 将数据传递给可视化器, 可视化器内封装了傅里叶变换与频域数据的缓动效果.

缓动效果也是自己封装的一个简单库, 只需要指定一些参数例如阻力, 回弹等, 即可实现缓动. (参考: [Bilibili: 如何用数学让动画变得极致丝滑？](https://www.bilibili.com/video/BV1wN4y1578b/))

<br/>

## 编译与使用

使用 Visual Studio 2022 打开项目, 并将配置改为 Release, 右键解决方案, 单击 “构建解决方案” 即可. 可执行文件会在 “项目目录/bin/Release/net6.0” 下.

如果需要单文件打包, 在右键项目, 单击 “发布”, 跟随向导, 创建一个发布到文件夹的配置, 并在发布页面中单击 “显示所有设置”, 在弹出的窗口中设置目标框架为 win-x64, 并将 “文件发布选项” 中的 “生成单文件” 勾选. 最后单击发布即可.

## 注意事项

项目基于 .NET6 开发, 运行程序需要保证电脑中已经安装 .NET6 Runtime. 如果发布时指定了目标框架为 win-x64, 那么用户也需要安装 64 位的 .NET6 桌面运行环境. win-x86 同理.