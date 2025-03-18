# 自定义端口运行

### 1. 源代码项目文件夹中运行  
> dotnet run --urls=http://*:5000  

### 2. 发布后的文件夹中运行：  
> dotnet RSAInteractJS.dll --urls=http://*:5000  

### 3. 发布后的文件夹中运行 for Lunux  
> export ASPNETCORE_URLS=http://*:5000  
> dotnet RSAInteractJS.dll  

### 4. 发布后的文件夹中运行 for Windows  
> set ASPNETCORE_URLS=http://*:5000  
> dotnet RSAInteractJS.dll  


# 生成 RSA 密钥对  
在 Linux 环境使用 openssl 生成密钥对
## 生成私钥  
```openssl genrsa -out private_key.pem 1024```
## 根据私钥生成公钥
```openssl rsa -in private_key.pem -outform PEM -pubout -out public_key.pem```

# 参考资料

## RSA 参考  

RSA PEM与XML格式转换  
https://www.ssleye.com/ssltool/pem_xml.html

生成 RSA 密钥对
https://www.ssleye.com/ssltool/pass_double.html

jsencrypt.js  
https://github.com/travist/jsencrypt

RSA 简介及 C# 和 js 实现  
https://blog.csdn.net/woisking2/article/details/131607958

RSA密钥之C#格式与Java格式转换  
https://www.cnblogs.com/datous/p/RSAKeyConvert.html

## SM2 参考  
https://github.com/Saberization/SM2
