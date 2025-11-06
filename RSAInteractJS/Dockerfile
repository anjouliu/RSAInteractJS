# 使用运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# 设置时区为上海
RUN cp /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
# 设置工作目录
WORKDIR /app
# 把目录下的内容都复制到当前目录下
COPY . .
# 暴露80端口
EXPOSE 8080
# 运行镜像入口命令和可执行文件名称
ENTRYPOINT ["dotnet", "RSAInteractJS.dll"]
