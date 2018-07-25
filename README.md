# 简单编译器

## 支持代码示例

```Simple C-like language
// This is a comment.
int integer;
int count;
count = 10;
output("Hello! Please enter an integer:\n");
input(integer);
if(integer > 100)
    integer = integer + 1;
else
    integer = integer - 1;
while(count > 0)
{
    integer = integer + 2;
    count = count - 1;
}
output("The result is: ");
output(integer);
output(".\n");
```

No complex expression.
No function calling.
No type conversion.

## 目前已完成

- Lexical analyzer
- Parser