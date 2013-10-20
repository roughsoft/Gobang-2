MULTIPLY MACRO X, Y		;乘法宏
			PUSH AX
			PUSH BX
			PUSH DX
			
			MOV AX, X				;将待乘两数分别存入AX,BX
			MOV BX, Y
			MOV DX, 00H
			MUL BX
			
			POP DX
			POP BX
			
			MOV X, AX
			POP AX
ENDM MULTIPLY

PRINTC MACRO X				;打印字符宏
			PUSH DX
			MOV DL, X				;将待打印字符的ASCII码存入DL
			CALL PRINT_CHAR	;调用字符打印过程
			POP DX
ENDM PRINTC

PRINTD MACRO X				;打印十六进制数字宏
			PUSH AX
			MOV AL, X				;将待打印十六进制数字存入AL
			CALL PRINT_DIGIT;调用十六进制数字打印过程
			POP AX
ENDM PRINTD

PRINTLN MACRO X				;换行宏
			CALL PRINT_ENDL	;调用换行过程
ENDM PRINTLN

PRINTS MACRO X				;打印字符串宏
			PUSH DX
			LEA DX, X				;将待打印字符串数组的地址存入DX
			CALL PRINT_STR	;调用字符串打印过程
			POP DX
ENDM PRINTS
			

DATA SEGMENT					;数据段
			LED	DB 3FH, 06H, 5BH, 4FH, 66H, 6DH, 7DH, 07H;控制各十六进制数数码管显示
					DB 7FH, 67H, 77H, 7CH, 39H, 5EH, 79H, 71H
			SEC DB 0				;当前秒
			MAT_COUNT DB 0	;点阵计数，决定是否显示白子
                        					
			TOP DB '  1 2 3 4 5 6 7 8 9 A B C D E F', 0DH, 0AH, '$';棋盘的列号
			TAB DB 05CH DUP(00H);棋盘
					DB 004H DUP(00H), 0C9H, 00DH DUP(0D1H), 0BBH, 004H DUP(00H)
					DB 00DH DUP(004H DUP(00H), 0C7H, 00DH DUP(0C5H), 0B6H, 004H DUP(00H))
					DB 004H DUP(00H), 0C8H, 00DH DUP(0CFH), 0BCH, 004H DUP(00H)
					DB 05CH DUP(00H)
			LINE DW 0				;统计当前落子处所在横/纵/斜/反斜的9个格子是否出现五子连
			
			ME DB 0					;本方棋子
			COUNT DB 0			;一共下的步数
			STOP DB 0				;是否停止，0代表不停止，1代表平局或负，2代表获胜
			
			YOU_WIN DB 0DH, 0AH, 'You win!', 0DH, 0AH, '$'
			YOU_LOSE DB 0DH, 0AH, 'You lose!', 0DH, 0AH, '$'
			DRAW_GAME DB 0DH, 0AH, 'Draw game!', 0DH, 0AH, '$'
			GIVE_UP DB ' Give up.$'
			
			WELCOME DB 'Welcome! Are you the first player?(Y/N)', 0DH, 0AH, '$'
			WARNING DB 'Invalid input! Please try again.', 0DH, 0AH, '$'
			TIMEOUT DB 0DH, 0AH, 'Timeout.$'
			
			PLAYER_TIP DB 'Player $';一些提示信息
			
			FREQ 	DW 140H, 140H, 155H, 180H, 180H, 155H, 140H, 120H, 100H, 100H, 120H, 140H, 140H, 120H, 120H
						DW 140H, 140H, 155H, 180H, 180H, 155H, 140H, 120H, 100H, 100H, 120H, 140H, 120H, 100H, 100H
						DW 120H, 120H, 140H, 100H, 120H, 140H, 155H, 140H, 100H, 120H, 140H, 155H, 140H, 120H, 100H, 120H, 0C0H, 140H
						DW 140H, 140H, 155H, 180H, 180H, 155H, 140H, 155H, 120H, 100H, 100H, 120H, 140H, 120H, 100H, 100H
			;欢乐颂每个音符的频率			
			TIME	DW 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 06H, 02H, 08H
						DW 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 06H, 02H, 08H
						DW 04H, 04H, 04H, 04H, 04H, 02H, 02H, 04H, 04H, 04H, 02H, 02H, 04H, 04H, 04H, 04H, 04H, 04H
						DW 04H, 04H, 04H, 04H, 04H, 04H, 04H, 02H, 02H, 04H, 04H, 04H, 04H, 06H, 02H, 08H
			;欢乐颂每个音符持续时间
DATA ENDS

STACK SEGMENT STACK
STACK ENDS

CODE SEGMENT
			ASSUME CS:CODE, DS:DATA
START:CALL INIT_8259;初始化中断控制器
			CALL INIT_8253;初始化定时/计数器
			CALL INIT_8251;初始化串行通信接口
			CALL INIT_8255;初始化并行通信接口
			
			CALL SELECT		;选择先后手
			CALL PRINT_TAB;打印棋盘
LO_MAIN:			
			MOV CL, 'X'		;黑子下
			CALL INPUT    ;输入棋子    
			CMP STOP, 00H	;是否结束？
			JNZ DOS				;结束返回DOS
			
			MOV CL, 'O'		;白子下
			CALL INPUT		;输入棋子
			CMP STOP, 00H	;是否结束？
			JZ LO_MAIN		;结束返回DOS
			
DOS:	CALL CLOSE_LED;关闭LED
			CALL CLOSE_8259;关闭中断控制器
			CMP STOP, 02H	;检测是否获胜
			JNZ NOT_WIN		;未获胜继续
			CALL MUSIC		;播放欢乐颂
			
NOT_WIN:
			MOV AH, 4CH		;返回DOS
			INT 21H

BEEP PROC NEAR			;响铃，调用微机内置8253、8255A
			PUSH AX
			PUSH BX
			PUSH CX
			
			MOV BL, 0FFH	;循环次数
			MOV AL, 0B6H	;设置内置8253计数器2，方式3，二进制位计数，先写低字节再写高字节
			OUT 43H, AL
			
			MOV AX, 500
			OUT 42H, AL		;输出计数初值低字节
			MOV AL, AH
			OUT 42H, AL		;输出计数初值高字节
			
			IN AL, 61H		;暂存微机8255A的B口数据
			MOV AH, AL
			OR AL, 03H		;设置8255A的B0,B1口为1
			OUT 61H, AL
			
			MOV CX, 00H
LO_BEEP:
			LOOP LO_BEEP	;循环256次
			DEC BL
			JNZ LO_BEEP		;循环255*256次
			
			MOV AL, AH		;将暂存的原微机8255A之B口数据恢复
			OUT 61H, AL
			
			POP CX
			POP BX
			POP AX
			RET
BEEP ENDP

CHECK_LINE PROC NEAR;检查五子棋生成的一行是否有五子连
			PUSH AX
LO_CHECK_LINE:			
			MOV AX, 1FH
			AND AX, LINE	;与11111B相与
			CMP AX, 1FH		;是否得到11111B
			JZ NEXT_CHECK_LINE;相等，有五子连，停止检查
			SHR LINE, 01H	;LINE右移一位
			JNZ LO_CHECK_LINE;直到LINE为0
NEXT_CHECK_LINE:
			POP AX
			RET
CHECK_LINE ENDP

CHECK_X PROC NEAR		;检查横行
			MOV LINE, 00H	;生成的行初设为0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;初始列数为落子位置左偏4格
			ADD AH, 04H		;行数为同一行
			
			MOV BX, CX		;暂存落子颜色
			MOV CX, 09H		;循环9次
LO_CHECK_X:
			CALL XLAT_2D	;查表
			INC AL				;右移一格
			SHL LINE, 01H	;生成的行左移一位
			CMP [SI], BL	;是否与落子同色
			JNZ NEXT_CHECK_X
			INC LINE			;相等则最低位加一
NEXT_CHECK_X:			
			LOOP LO_CHECK_X
			
			POP SI
			POP CX
			POP BX
			POP AX
			
			CALL CHECK_LINE
			RET
CHECK_X ENDP

CHECK_Y PROC NEAR		;检查纵列
			MOV LINE, 00H	;生成的行初设为0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;初始行数为落子位置上4格
			ADD AL, 04H		;列数为落子同一列
			
			MOV BX, CX
			MOV CX, 09H
LO_CHECK_Y:
			CALL XLAT_2D
			INC AH				;检查位置下移一格，其他地方同横行检查
			SHL LINE, 01H
			CMP [SI], BL
			JNZ NEXT_CHECK_Y
			INC LINE
NEXT_CHECK_Y:			
			LOOP LO_CHECK_Y
			
			POP SI
			POP CX
			POP BX
			POP AX
			
			CALL CHECK_LINE
			RET
CHECK_Y ENDP

CHECK_XY PROC NEAR	;检查斜线
			MOV LINE, 00H	;生成的行初设为0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;初始位置在落子位置左上四格
			
			MOV BX, CX
			MOV CX, 09H
LO_CHECK_XY:
			CALL XLAT_2D
			ADD AX, 0101H	;检查位置右下移一格，其他地方同横行检查
			SHL LINE, 01H
			CMP [SI], BL
			JNZ NEXT_CHECK_XY
			INC LINE
NEXT_CHECK_XY:			
			LOOP LO_CHECK_XY
			
			POP SI
			POP CX
			POP BX
			POP AX
			
			CALL CHECK_LINE
			RET
CHECK_XY ENDP

CHECK_X_Y PROC NEAR	;检查反斜线
			MOV LINE, 00H	;生成的行初设为0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;初始位置在落子位置右上四格
			ADD AL, 08H

			MOV BX, CX
			MOV CX, 09H
LO_CHECK_X_Y:
			CALL XLAT_2D
			INC AH				;检查位置右下移一格，其他地方同横行检查
			DEC AL
			SHL LINE, 01H
			CMP [SI], BL
			JNZ NEXT_CHECK_X_Y
			INC LINE
NEXT_CHECK_X_Y:			
			LOOP LO_CHECK_X_Y
			
			POP SI
			POP CX
			POP BX
			POP AX
			
			CALL CHECK_LINE
			RET
CHECK_X_Y ENDP

CLEAR_KEY PROC NEAR		;清空键盘缓冲
			PUSH AX
LO_CLEAR_KEY:
			MOV AH, 01H
			INT 16H					;键盘缓冲区是否为空
			JZ RET_CLEAR_KEY
			MOV AH, 00H
			INT 16H					;非空则读入字符
			JMP LO_CLEAR_KEY
RET_CLEAR_KEY:			
			POP AX
			RET
CLEAR_KEY ENDP

CLOSE_8259 PROC NEAR	;退出8259中断控制器
			PUSH AX
			PUSH DX
			
			IN AL, 21H
			OR AL, 40H		;关闭IRQ6
			OUT 21H, AL
						
			MOV DX, 0EC4CH
			MOV AL, 42H		;关闭PCI9052中断
			OUT DX, AL
			STI
			
			POP DX
			POP AX
			RET
CLOSE_8259 ENDP

CLOSE_LED PROC NEAR	;关闭LED(数码管和点阵)
			PUSH AX
			PUSH DX
			
			MOV DX, 0E4A8H;关闭点阵下八行显示
			MOV AL, 00H
			OUT DX, AL
			
			MOV DX, 0E4A9H;关闭点阵上八行显示
			MOV AL, 00H
			OUT DX, AL
			
			MOV DX, 0E48AH;关闭所有数码管
			MOV AL, 00H
			OUT DX, AL
			
			POP DX
			POP AX
			RET
CLOSE_LED ENDP

CLS PROC NEAR				;清屏
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH DX
			
			MOV AH, 02H
			MOV BH, 00H
			MOV DX, 00H
			INT 10H				;改变光标位置至左上角
			
			MOV CX, 07D0H	;打印80*25个空格
LO_CLS:			
			PRINTC ' '
			LOOP LO_CLS
			
			MOV AH, 02H
			MOV BH, 00H
			MOV DX, 00H
			INT 10H				;改变光标位置至左上角
			
			POP DX
			POP CX
			POP BX
			POP AX
			RET
CLS ENDP

DISP_SEC PROC NEAR	;数码管显示秒钟
			PUSH AX
			PUSH BX
			PUSH DX
	
			LEA BX, LED
			
			CALL CLOSE_LED;关闭数码管
	
			MOV DX, 0E488H
			MOV AL, 01H		;最右边数码管亮
			OUT DX, AL		;输出位码
		
			MOV DX, 0E48AH
			MOV AL, SEC
			AND AL, 0FH
			XLAT					;获得秒钟个位段码
			OUT DX, AL		;输出段码
	
			CALL CLOSE_LED;关闭数码管
	
			MOV DX, 0E488H
			MOV AL, 02H		;次右边数码管亮
			OUT DX, AL		;输出位码
	
			MOV DX, 0E48AH
			MOV AL, SEC

			PUSH CX
			MOV CL, 04H
			SHR AL, CL
			POP CX

			XLAT					;获得秒钟十位段码
			OUT DX, AL		;输出段码
	
			POP DX
			POP BX
			POP AX
			RET
DISP_SEC ENDP

INIT_8251 PROC NEAR	;初始化8251
			PUSH AX
			PUSH DX
			
			MOV DX, 0E4B9H
		 	   
			MOV AL, 0
			OUT DX, AL
			OUT DX, AL
			OUT DX, AL
			
			MOV AL, 40H		;内部复位
			OUT DX, AL
			MOV AL, 4EH		;方式字：异步方式，1位停止位，8位数据位，无奇偶校验位，波特率因子16
			OUT DX, AL
			MOV AL, 27H		;命令字，请求发送，允许发送和接收，数据终端准备好
			OUT DX, AL
   	     
			POP DX
			POP AX
			RET
INIT_8251 ENDP

INIT_8253 PROC NEAR	;初始化8253
			PUSH AX
			PUSH DX
			
			MOV DX, 0E483H;计数器0，方式3，二进制计数，写低字节
			MOV AL, 16H
			OUT DX, AL

			MOV DX, 0E480H
			MOV AL, 3AH		;计数初值52
			OUT DX, AL
			
			POP DX
			POP AX
			RET
INIT_8253 ENDP

INIT_8255 PROC NEAR	;初始化8255A
			PUSH AX
			PUSH DX
			
			MOV DX, 0E48BH;8255A芯片各口设置输出 
			MOV AL, 80H
			OUT DX, AL
			
			MOV DX, 0E4ABH;LED点阵板8255A各口设置输出
			MOV AL, 80H
			OUT DX, AL
			
			POP DX
			POP AX
			RET
INIT_8255 ENDP

INIT_8259 PROC NEAR	;初始化8259
			PUSH AX
			PUSH DX
			
			MOV AX, CS
			MOV DS, AX
			MOV DX, OFFSET IRQ	
			MOV AX, 250EH
			INT 21H				;置中断向量
			CLI
			
			MOV DX, 0EC4CH
			MOV AL, 43H		;开中断
			OUT DX, AL
			
			INC DX
			MOV AL, 1DH		;清除中断标志
			OUT DX, AL
			
			IN AL, 21H
			AND AL, 0BFH	;开IRQ6中断
			OUT 21H, AL
			
			MOV AX, DATA
			MOV DS, AX
			STI
			
			POP DX
			POP AX
			RET
INIT_8259 ENDP

INIT_CLOCK PROC NEAR;初始化时钟的8253
			PUSH AX
			PUSH DX
			
			MOV SEC, 60H	;时间60秒
			
			MOV DX, 0E483H;计数器1，方式3，二进制计数，先写低字节再写高字节
			MOV AL, 76H
			OUT DX, AL

			MOV DX, 0E481H
			MOV AL, 0E8H	;1000的十六进制低字节
			OUT DX, AL
			
			MOV DX, 0E481H
			MOV AL, 03H		;1000的十六进制高字节
			OUT DX, AL
			
			MOV DX, 0E483H;计数器2，方式3，二进制计数，先写低字节再写高字节
			MOV AL, 0B6H
			OUT DX, AL

			MOV DX, 0E482H
			MOV AL, 0E8H
			OUT DX, AL
			
			MOV DX, 0E482H
			MOV AL, 03H
			OUT DX, AL
			
			MOV DI, 00H		;未发生中断

			POP DX
			POP AX
			RET
INIT_CLOCK ENDP

INPUT PROC NEAR			;输入
			CALL INIT_CLOCK;初始化时钟
			
			CMP CL, ME		;比较是否为本机下
			JZ IN_ME
			CALL INPUT_ENEMY;对手下
			RET
IN_ME:CALL INPUT_ME	;自己下
			RET
INPUT ENDP

INPUT_ENEMY PROC NEAR;对手下
			CALL RECEIVE	;调用接收函数
			CALL CLEAR_KEY;清除键盘缓冲
			CALL PUT_CHESS;落子
			RET
INPUT_ENEMY ENDP

INPUT_ME PROC NEAR	;自己下
			PUSH AX
			PUSH BX
			PUSH SI
			
LO_INPUT_ME1:
			PRINTS PLAYER_TIP;玩家提示
			PRINTC CL			;输出自己的子样式
			PRINTC 3AH		;输出冒号
			
			PUSH CX
			MOV CX, 02H		;接收两位十六进制数字
			MOV DX, 00H		;输入的初始为0
LO_INPUT_ME2:
			MOV DH, DL		;将低8位移到高8位
LO_INPUT_ME3:
			CALL SHOW_TAB	;在点阵板显示棋子
			CALL DISP_SEC	;在数码管显示倒计时
			CMP DI, 01H		;检测是否发生中断
			JNZ NOIRQ_INPUT_ME
			
			MOV AL, SEC		;秒钟减一，BCD计数
			DEC AL
			DAS
			MOV SEC, AL
			
			MOV DI, 00H		;清除中断
			
			CMP SEC, 10H	;是否是倒数10秒
			JA NOBEEP_INPUT_ME
			CALL BEEP			;响铃报警
			
NOBEEP_INPUT_ME:			
			CMP SEC, 00H	;是否到时
			JNZ NOIRQ_INPUT_ME
			JMP TIMEOUT_INPUT_ME;跳至超时
NOIRQ_INPUT_ME:
			MOV AH, 01H		;检测键盘缓冲区是否有数据输入
			INT 16H
			JZ LO_INPUT_ME3
			
			MOV AH, 07H		;输入内容
			INT 21H
			
			CMP AL, 1BH		;是否为ESC
			JNZ NEXT_INPUT_ME0
			JMP GIVE_UP_INPUT_ME;ESC弃权
NEXT_INPUT_ME0:			
			CMP AL, 08H		;是否为退格
			JZ BKSP_INPUT_ME
			
			CMP AL, 31H		;是否小于'1'
			JB LO_INPUT_ME3
			
			MOV BL, AL
			
			SUB AL, 30H		;减去'0'
			CMP AL, 0AH		;是否小于0AH
			JB NEXT_INPUT_ME1
			
			SUB AL, 07H
			JB LO_INPUT_ME3
			
			CMP AL, 0AH		;是否大于0AH
			JB LO_INPUT_ME3
			
			CMP AL, 10H		;是否小于10H（大写字母）
			JB NEXT_INPUT_ME1
			
			SUB AL, 20H		;减去20H
			JB LO_INPUT_ME3
			
			CMP AL, 0AH		;是否大于0AH
			JB LO_INPUT_ME3
			
			CMP AL, 0FH		;是否小于10H（小写字母）
			JA LO_INPUT_ME3
			JMP NEXT_INPUT_ME1

BKSP_INPUT_ME:			;退格
			CMP CX, 02H		;是否未输入
			JZ LO_INPUT_ME3
			
			PRINTC AL
			PRINTC ' '
			PRINTC AL			;退格覆盖掉原数
			
			MOV DX, 00H		;清除位置
			MOV CX, 02H		;已输入数重为0
			JMP LO_INPUT_ME3

NEXT_INPUT_ME1:
			MOV DL, AL		;存入输入的位置
			PRINTC BL			;打印字符
			
			DEC CX				;计数减一
			JZ NEXT_INPUT_ME2
			JMP LO_INPUT_ME2
			
NEXT_INPUT_ME2:			
			POP CX
			PRINTLN				;换行
			
			MOV AX, DX
			ADD AX, 0404H
			CALL XLAT_2D
			
			CMP BYTE PTR [SI], 'X';检查是否要下的地方已落子
			JZ NEXT_INPUT_ME3
			CMP BYTE PTR [SI], 'O';检查是否要下的地方已落子
			JZ NEXT_INPUT_ME3
			
			JMP RET_INPUT_ME
NEXT_INPUT_ME3:
			PRINTS WARNING;已落子则警告
			JMP LO_INPUT_ME1

TIMEOUT_INPUT_ME:		;超时
			PRINTS TIMEOUT
GIVE_UP_INPUT_ME:		;弃权
			POP CX
			MOV DX, 00H
			
RET_INPUT_ME:
			POP SI
			POP BX
			POP AX
			
			CALL PUT_CHESS;落子
			RET
INPUT_ME ENDP

MUSIC PROC NEAR			;播放欢乐颂
			PUSH SI
			MOV SI, 00H		;第一个音符开始
LO_MUSIC:
			CALL PLAY			;播放
			
			MOV AH, 01H		;如果有按键输入结束播放
			INT 16H
			JNZ RET_MUSIC
			
			ADD SI, 02H		;偏移地址加2
			
			CMP SI, 80H		;直到结束
			JB LO_MUSIC
RET_MUSIC:
			POP SI
			RET
MUSIC ENDP

PLAY PROC NEAR			;播放一个音符
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH DX
			
			MOV AL, 0B6H	;设置内置8253计数器2，方式3，二进制位计数，先写低字节再写高字节
			OUT 43H, AL	
			
			MOV AX, 34DDH
			MOV BX, FREQ[SI];获取音符频率
			MOV DX, 12H
			DIV BX				;用1193181Hz除以音符频率
			
			OUT 42H, AL		;输出低字节
			MOV AL, AH
			OUT 42H, AL		;输出高字节

			IN AL, 61H		;暂存微机8255A的B口数据
			MOV BL, AL
			OR AL, 03H		;设置8255A的B0,B1口为1
			OUT 61H, AL

			MOV AH, 86H		;延时指定时间
			MOV CX, TIME[SI]
			MOV DX, 00H
			INT 15H
			
			MOV AL, BL		;将暂存的原微机8255A之B口数据恢复
			OUT 61H, AL
			
			POP DX
			POP CX
			POP BX
			POP AX
			RET
PLAY ENDP

PRINT_CHAR PROC NEAR;打印字符
			PUSH AX
			MOV AH, 02H
			INT 21H				;输出DL存储字符
			POP AX
			RET
PRINT_CHAR ENDP

PRINT_DIGIT PROC NEAR;打印十六进制数字
			PUSH DX
			MOV DL, AL
			CALL TO_ASC		;将DL中数字转为ASCII码
			CALL PRINT_CHAR;打印字符
			POP DX
			RET
PRINT_DIGIT ENDP

PRINT_ENDL PROC NEAR;换行
			PUSH AX
			PUSH DX
			MOV AH, 02H
			
			MOV DL, 0DH		;回车符
			INT 21H
			
			MOV DL, 0AH		;换行符
			INT 21H
			
			POP DX
			POP AX
			RET
PRINT_ENDL ENDP

PRINT_STR PROC NEAR	;打印字符
			PUSH AX
			MOV AH, 09H		;打印DX中的字符串
			INT 21H
			POP AX
			RET			
PRINT_STR ENDP

PRINT_TAB PROC NEAR	;打印棋盘
			PUSH AX
			PUSH CX
			PUSH DX
			PUSH SI
			
			CALL CLS			;清屏
			PRINTS TOP		;打印首行
			
			MOV AL, 00H
			LEA SI, TAB
			ADD SI, 57H
LO_TAB0:
			INC AL
			ADD SI, 08H
			CALL PRINT_DIGIT;打印行号
			
			MOV CX, 0FH
LO_TAB1:
			INC SI
			
			CMP CX, 0FH
			JNZ NEXT_TAB0
			
			PRINTC ' '
			JMP NEXT_TAB2
NEXT_TAB0:					;各种制表符
			CMP AL, 01H
			JZ NEXT_TAB1
			CMP AL, 0FH
			JZ NEXT_TAB1
			
			PRINTC 0C4H
			JMP NEXT_TAB2
NEXT_TAB1:
			PRINTC 0CDH
NEXT_TAB2:			
			PRINTC [SI]		;打印棋盘内容
			
			LOOP LO_TAB1
			
			PRINTLN				;换行
			CMP AL, 0FH		;一共十五行
			JNZ LO_TAB0
			
			POP SI
			POP DX
			POP CX
			POP AX
			RET
PRINT_TAB ENDP

PUT_CHESS PROC NEAR	;落子
			CMP CL, ME
			JNZ DO_NOT_SEND
			
			CALL SEND			;本方落子后要发送
DO_NOT_SEND:
			CMP DX, 00H		;是否弃权
			JZ GIVE_UP_PUT_CHESS	
			
			INC COUNT			;计数加一
			
			PUSH AX
			PUSH SI
			
			MOV AX, DX
			ADD AX, 0404H
			CALL XLAT_2D
			MOV [SI], CL	;将落的子放入指定位置
			
			POP SI
			POP AX
			
			CALL PRINT_TAB;输出棋盘
			
			PRINTS PLAYER_TIP;输出玩家提示
			PRINTC CL			;输出玩家棋子样式
			PRINTC 3AH		;输出冒号
			PRINTD DH			;输出落子行号
			PRINTD DL			;输出落子列号
			PRINTLN				;输出换行
			
			CALL CHECK_X	;检查横行
			CMP LINE, 00H
			JNZ ENDGAME
			
			CALL CHECK_Y	;检查纵列
			CMP LINE, 00H
			JNZ ENDGAME
			
			CALL CHECK_XY	;检查斜线
			CMP LINE, 00H
			JNZ ENDGAME
			
			CALL CHECK_X_Y;检查反斜线
			CMP LINE, 00H
			JNZ ENDGAME			
			
			JMP NOT_ENDGAME

ENDGAME:			
			CMP ME, CL		;是否为自己
			JZ WIN				;是则胜
			JMP LOSE			;否则负
			
GIVE_UP_PUT_CHESS:	;弃权
			PRINTLN
			PRINTC CL
			PRINTS GIVE_UP;输出提示
			
			CMP ME, CL		;是否为自己
			JNZ WIN				;是则负
			JMP LOSE			;否则胜

WIN:	PRINTS YOU_WIN
			MOV STOP, 02H	;设置结束
			JMP RET_PUT_CHESS

LOSE:	PRINTS YOU_LOSE
			MOV STOP, 01H	;设置结束
			JMP RET_PUT_CHESS

NOT_ENDGAME:
			CMP COUNT, 0E1H;是否下225个子
			JNZ RET_PUT_CHESS
			
			MOV STOP, 01H	;结束
			PRINTS DRAW_GAME

RET_PUT_CHESS:
			RET
PUT_CHESS ENDP

READ_CHAR PROC NEAR	;读取按键，存于AL
			PUSH BX
			MOV BH, AH
			
			MOV AH, 01H
			INT 21H
			
			MOV AH, BH
			POP BX
			RET
READ_CHAR ENDP

RECEIVE PROC NEAR		;查询方式接收
			PUSH AX        
			MOV DX, 0E4B9H
LO_RECEIVE:
			CALL SHOW_TAB	;在点阵板显示棋子
			CALL DISP_SEC	;在数码管显示倒计时
			CMP DI, 01H		;是否中断
			JNZ NOIRQ_RECEIVE
			
			MOV AL, SEC		;秒钟减一，BCD计数
      DEC AL
			DAS
      MOV SEC, AL
			
			MOV DI, 00H		;清除中断
			
			CMP SEC, 00H	;是否超时
			JZ TIMEOUT_RECEIVE
NOIRQ_RECEIVE:
			IN AL, DX
			TEST AL, 02H
			JZ LO_RECEIVE	;检查是否接受就绪
        
			MOV DX, 0E4B8H
			IN AL, DX			;接收落子
      
			MOV DH, AL		;将八位数字还原为十六位数字
			MOV DL, AL

      PUSH CX
      MOV CL, 04H
      SHR DH, CL
      POP CX

      AND DL, 0FH
        
      POP AX
      RET
TIMEOUT_RECEIVE:
			PRINTS TIMEOUT;打印超时
			MOV DX, 00H		;落子置为弃权标志
			
			POP AX
			RET
RECEIVE ENDP

SELECT PROC NEAR		;选择先后手
			PUSH AX
			
			PRINTS WELCOME
LO_SELECT:
			CALL READ_CHAR
			PRINTLN
			
			CMP AL, 'Y'
			JZ FIRST
			CMP AL, 'y'
			JZ FIRST			;先手
			
			CMP AL, 'N'
			JZ SECOND
			CMP AL, 'n'
			JZ SECOND			;后手
			
			PRINTS WARNING;警告后重试
			JMP LO_SELECT
FIRST:MOV ME, 'X'		;先手棋子为'X'
			POP AX
			RET
SECOND:
			MOV ME, 'O'		;后手棋子为'O'
			POP AX
			RET
SELECT ENDP

SEND PROC NEAR
			PUSH AX
			PUSH DX
        
			MOV DX, 0E4B9H
LO_SEND:IN AL, DX
			TEST AL, 01H	;发送缓冲区是否为空
			JZ LO_SEND
                        
			POP DX

			MOV AL, DH		;将十六位数字压缩为八位数字
			PUSH CX
			MOV CL, 04H
			SHL AL, CL
			POP CX

			ADD AL, DL

			PUSH DX  
			MOV DX, 0E4B8H
			OUT DX, AL		;发送位置
        
			POP DX
			POP AX
			RET
SEND ENDP

SHOW_TAB PROC NEAR	;点阵板显示棋盘
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH DX
			PUSH SI
			
			MOV AL, 05H		;从左上角开始落子
LO_SHOW_TAB1:
			MOV AH, 05H
			CALL CLOSE_LED;关LED
			
			MOV DX, 0E4AAH
			SUB AL, 05H
			OUT DX, AL		;输出列号
			ADD AL, 05H
			
			MOV BX, 00H
LO_SHOW_TAB2:
			SHL BX, 01H		;BX左移一位
			CALL XLAT_2D
			
			CMP BYTE PTR [SI], 'X'
			JZ SHOW_CHESS1;棋子为X，显示在点阵板上
			CMP BYTE PTR [SI], 'O'
			JNZ NOT_SHOW_CHESS1
			CMP MAT_COUNT, 00H;棋子为O且MATCOUNT计数为0，显示在点阵板上
			JNZ NOT_SHOW_CHESS1
SHOW_CHESS1:
			INC BX
NOT_SHOW_CHESS1:
			INC AH
			CMP AH, 0DH		;前八行
			JB LO_SHOW_TAB2
			
			PUSH AX
			MOV DX, 0E4A9H;控制点阵板上八行
			MOV AL, BL
			OUT DX, AL
			POP AX
			
			MOV BX, 00H
LO_SHOW_TAB3:
			SHL BX, 01H		;BX左移一位
			CALL XLAT_2D
			
			CMP BYTE PTR [SI], 'X'
			JZ SHOW_CHESS2;棋子为X，显示在点阵板上
			CMP BYTE PTR [SI], 'O'
			JNZ NOT_SHOW_CHESS2
			CMP MAT_COUNT, 00H
			JNZ NOT_SHOW_CHESS2;棋子为O且MATCOUNT计数为0，显示在点阵板上
			
SHOW_CHESS2:
			INC BX
NOT_SHOW_CHESS2:
			INC AH
			CMP AH, 14H		;后七行
			JB LO_SHOW_TAB3
			
			PUSH AX
			MOV DX, 0E4A8H;控制点阵板下八行
			MOV AL, BL
			SHL AL, 01H		;再左移一次
			OUT DX, AL
			POP AX
			
			INC AL
			CMP AL, 14H		;依次输出15个列
			JB LO_SHOW_TAB1
			
			INC MAT_COUNT
			AND MAT_COUNT, 07H;MAT_COUNT在0~7循环
			
			POP SI
			POP DX
			POP CX
			POP BX
			POP AX
			RET
SHOW_TAB ENDP

TO_ASC PROC NEAR		;转换ASCII码
			ADD DL, 30H		;加'0'
			CMP DL, 39H		;超过'9'?
			JBE RET_ASC
			ADD DL, 07H		;加07H	
RET_ASC:
			RET
TO_ASC ENDP

XLAT_2D PROC NEAR		;查二维数组，AH为行号，AL为列号，左上角为0101H
			PUSH AX
			PUSH DX
			
			MOV DH, 00H
			MOV DL, AH		;行号减一
			DEC DL
			MULTIPLY DX, 17H;乘以23
			
			MOV AH, 00H
			ADD DX, AX		;加列号
			DEC DX				;减一
			
			LEA SI, TAB		;查表
			ADD SI, DX		;加偏移
			
			POP DX
			POP AX
			RET
XLAT_2D ENDP

IRQ PROC FAR				;中断服务程序
			PUSH AX
			PUSH DX
			
			MOV DI, 01H		;提示有中断
			
			MOV AL, 20H
			OUT 20H, AL
			
			MOV DX, 0EC4DH
			MOV AL, 1DH		;清除PCI9052的中断标志
			OUT DX, AL
			
			POP DX
			POP AX
			IRET
IRQ ENDP	

CODE ENDS
	END START
