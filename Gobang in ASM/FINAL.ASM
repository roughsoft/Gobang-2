MULTIPLY MACRO X, Y		;�˷���
			PUSH AX
			PUSH BX
			PUSH DX
			
			MOV AX, X				;�����������ֱ����AX,BX
			MOV BX, Y
			MOV DX, 00H
			MUL BX
			
			POP DX
			POP BX
			
			MOV X, AX
			POP AX
ENDM MULTIPLY

PRINTC MACRO X				;��ӡ�ַ���
			PUSH DX
			MOV DL, X				;������ӡ�ַ���ASCII�����DL
			CALL PRINT_CHAR	;�����ַ���ӡ����
			POP DX
ENDM PRINTC

PRINTD MACRO X				;��ӡʮ���������ֺ�
			PUSH AX
			MOV AL, X				;������ӡʮ���������ִ���AL
			CALL PRINT_DIGIT;����ʮ���������ִ�ӡ����
			POP AX
ENDM PRINTD

PRINTLN MACRO X				;���к�
			CALL PRINT_ENDL	;���û��й���
ENDM PRINTLN

PRINTS MACRO X				;��ӡ�ַ�����
			PUSH DX
			LEA DX, X				;������ӡ�ַ�������ĵ�ַ����DX
			CALL PRINT_STR	;�����ַ�����ӡ����
			POP DX
ENDM PRINTS
			

DATA SEGMENT					;���ݶ�
			LED	DB 3FH, 06H, 5BH, 4FH, 66H, 6DH, 7DH, 07H;���Ƹ�ʮ���������������ʾ
					DB 7FH, 67H, 77H, 7CH, 39H, 5EH, 79H, 71H
			SEC DB 0				;��ǰ��
			MAT_COUNT DB 0	;��������������Ƿ���ʾ����
                        					
			TOP DB '  1 2 3 4 5 6 7 8 9 A B C D E F', 0DH, 0AH, '$';���̵��к�
			TAB DB 05CH DUP(00H);����
					DB 004H DUP(00H), 0C9H, 00DH DUP(0D1H), 0BBH, 004H DUP(00H)
					DB 00DH DUP(004H DUP(00H), 0C7H, 00DH DUP(0C5H), 0B6H, 004H DUP(00H))
					DB 004H DUP(00H), 0C8H, 00DH DUP(0CFH), 0BCH, 004H DUP(00H)
					DB 05CH DUP(00H)
			LINE DW 0				;ͳ�Ƶ�ǰ���Ӵ����ں�/��/б/��б��9�������Ƿ����������
			
			ME DB 0					;��������
			COUNT DB 0			;һ���µĲ���
			STOP DB 0				;�Ƿ�ֹͣ��0������ֹͣ��1����ƽ�ֻ򸺣�2������ʤ
			
			YOU_WIN DB 0DH, 0AH, 'You win!', 0DH, 0AH, '$'
			YOU_LOSE DB 0DH, 0AH, 'You lose!', 0DH, 0AH, '$'
			DRAW_GAME DB 0DH, 0AH, 'Draw game!', 0DH, 0AH, '$'
			GIVE_UP DB ' Give up.$'
			
			WELCOME DB 'Welcome! Are you the first player?(Y/N)', 0DH, 0AH, '$'
			WARNING DB 'Invalid input! Please try again.', 0DH, 0AH, '$'
			TIMEOUT DB 0DH, 0AH, 'Timeout.$'
			
			PLAYER_TIP DB 'Player $';һЩ��ʾ��Ϣ
			
			FREQ 	DW 140H, 140H, 155H, 180H, 180H, 155H, 140H, 120H, 100H, 100H, 120H, 140H, 140H, 120H, 120H
						DW 140H, 140H, 155H, 180H, 180H, 155H, 140H, 120H, 100H, 100H, 120H, 140H, 120H, 100H, 100H
						DW 120H, 120H, 140H, 100H, 120H, 140H, 155H, 140H, 100H, 120H, 140H, 155H, 140H, 120H, 100H, 120H, 0C0H, 140H
						DW 140H, 140H, 155H, 180H, 180H, 155H, 140H, 155H, 120H, 100H, 100H, 120H, 140H, 120H, 100H, 100H
			;������ÿ��������Ƶ��			
			TIME	DW 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 06H, 02H, 08H
						DW 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 04H, 06H, 02H, 08H
						DW 04H, 04H, 04H, 04H, 04H, 02H, 02H, 04H, 04H, 04H, 02H, 02H, 04H, 04H, 04H, 04H, 04H, 04H
						DW 04H, 04H, 04H, 04H, 04H, 04H, 04H, 02H, 02H, 04H, 04H, 04H, 04H, 06H, 02H, 08H
			;������ÿ����������ʱ��
DATA ENDS

STACK SEGMENT STACK
STACK ENDS

CODE SEGMENT
			ASSUME CS:CODE, DS:DATA
START:CALL INIT_8259;��ʼ���жϿ�����
			CALL INIT_8253;��ʼ����ʱ/������
			CALL INIT_8251;��ʼ������ͨ�Žӿ�
			CALL INIT_8255;��ʼ������ͨ�Žӿ�
			
			CALL SELECT		;ѡ���Ⱥ���
			CALL PRINT_TAB;��ӡ����
LO_MAIN:			
			MOV CL, 'X'		;������
			CALL INPUT    ;��������    
			CMP STOP, 00H	;�Ƿ������
			JNZ DOS				;��������DOS
			
			MOV CL, 'O'		;������
			CALL INPUT		;��������
			CMP STOP, 00H	;�Ƿ������
			JZ LO_MAIN		;��������DOS
			
DOS:	CALL CLOSE_LED;�ر�LED
			CALL CLOSE_8259;�ر��жϿ�����
			CMP STOP, 02H	;����Ƿ��ʤ
			JNZ NOT_WIN		;δ��ʤ����
			CALL MUSIC		;���Ż�����
			
NOT_WIN:
			MOV AH, 4CH		;����DOS
			INT 21H

BEEP PROC NEAR			;���壬����΢������8253��8255A
			PUSH AX
			PUSH BX
			PUSH CX
			
			MOV BL, 0FFH	;ѭ������
			MOV AL, 0B6H	;��������8253������2����ʽ3��������λ��������д���ֽ���д���ֽ�
			OUT 43H, AL
			
			MOV AX, 500
			OUT 42H, AL		;���������ֵ���ֽ�
			MOV AL, AH
			OUT 42H, AL		;���������ֵ���ֽ�
			
			IN AL, 61H		;�ݴ�΢��8255A��B������
			MOV AH, AL
			OR AL, 03H		;����8255A��B0,B1��Ϊ1
			OUT 61H, AL
			
			MOV CX, 00H
LO_BEEP:
			LOOP LO_BEEP	;ѭ��256��
			DEC BL
			JNZ LO_BEEP		;ѭ��255*256��
			
			MOV AL, AH		;���ݴ��ԭ΢��8255A֮B�����ݻָ�
			OUT 61H, AL
			
			POP CX
			POP BX
			POP AX
			RET
BEEP ENDP

CHECK_LINE PROC NEAR;������������ɵ�һ���Ƿ���������
			PUSH AX
LO_CHECK_LINE:			
			MOV AX, 1FH
			AND AX, LINE	;��11111B����
			CMP AX, 1FH		;�Ƿ�õ�11111B
			JZ NEXT_CHECK_LINE;��ȣ�����������ֹͣ���
			SHR LINE, 01H	;LINE����һλ
			JNZ LO_CHECK_LINE;ֱ��LINEΪ0
NEXT_CHECK_LINE:
			POP AX
			RET
CHECK_LINE ENDP

CHECK_X PROC NEAR		;������
			MOV LINE, 00H	;���ɵ��г���Ϊ0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;��ʼ����Ϊ����λ����ƫ4��
			ADD AH, 04H		;����Ϊͬһ��
			
			MOV BX, CX		;�ݴ�������ɫ
			MOV CX, 09H		;ѭ��9��
LO_CHECK_X:
			CALL XLAT_2D	;���
			INC AL				;����һ��
			SHL LINE, 01H	;���ɵ�������һλ
			CMP [SI], BL	;�Ƿ�������ͬɫ
			JNZ NEXT_CHECK_X
			INC LINE			;��������λ��һ
NEXT_CHECK_X:			
			LOOP LO_CHECK_X
			
			POP SI
			POP CX
			POP BX
			POP AX
			
			CALL CHECK_LINE
			RET
CHECK_X ENDP

CHECK_Y PROC NEAR		;�������
			MOV LINE, 00H	;���ɵ��г���Ϊ0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;��ʼ����Ϊ����λ����4��
			ADD AL, 04H		;����Ϊ����ͬһ��
			
			MOV BX, CX
			MOV CX, 09H
LO_CHECK_Y:
			CALL XLAT_2D
			INC AH				;���λ������һ�������ط�ͬ���м��
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

CHECK_XY PROC NEAR	;���б��
			MOV LINE, 00H	;���ɵ��г���Ϊ0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;��ʼλ��������λ�������ĸ�
			
			MOV BX, CX
			MOV CX, 09H
LO_CHECK_XY:
			CALL XLAT_2D
			ADD AX, 0101H	;���λ��������һ�������ط�ͬ���м��
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

CHECK_X_Y PROC NEAR	;��鷴б��
			MOV LINE, 00H	;���ɵ��г���Ϊ0
			
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH SI
			
			MOV AX, DX		;��ʼλ��������λ�������ĸ�
			ADD AL, 08H

			MOV BX, CX
			MOV CX, 09H
LO_CHECK_X_Y:
			CALL XLAT_2D
			INC AH				;���λ��������һ�������ط�ͬ���м��
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

CLEAR_KEY PROC NEAR		;��ռ��̻���
			PUSH AX
LO_CLEAR_KEY:
			MOV AH, 01H
			INT 16H					;���̻������Ƿ�Ϊ��
			JZ RET_CLEAR_KEY
			MOV AH, 00H
			INT 16H					;�ǿ�������ַ�
			JMP LO_CLEAR_KEY
RET_CLEAR_KEY:			
			POP AX
			RET
CLEAR_KEY ENDP

CLOSE_8259 PROC NEAR	;�˳�8259�жϿ�����
			PUSH AX
			PUSH DX
			
			IN AL, 21H
			OR AL, 40H		;�ر�IRQ6
			OUT 21H, AL
						
			MOV DX, 0EC4CH
			MOV AL, 42H		;�ر�PCI9052�ж�
			OUT DX, AL
			STI
			
			POP DX
			POP AX
			RET
CLOSE_8259 ENDP

CLOSE_LED PROC NEAR	;�ر�LED(����ܺ͵���)
			PUSH AX
			PUSH DX
			
			MOV DX, 0E4A8H;�رյ����°�����ʾ
			MOV AL, 00H
			OUT DX, AL
			
			MOV DX, 0E4A9H;�رյ����ϰ�����ʾ
			MOV AL, 00H
			OUT DX, AL
			
			MOV DX, 0E48AH;�ر����������
			MOV AL, 00H
			OUT DX, AL
			
			POP DX
			POP AX
			RET
CLOSE_LED ENDP

CLS PROC NEAR				;����
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH DX
			
			MOV AH, 02H
			MOV BH, 00H
			MOV DX, 00H
			INT 10H				;�ı���λ�������Ͻ�
			
			MOV CX, 07D0H	;��ӡ80*25���ո�
LO_CLS:			
			PRINTC ' '
			LOOP LO_CLS
			
			MOV AH, 02H
			MOV BH, 00H
			MOV DX, 00H
			INT 10H				;�ı���λ�������Ͻ�
			
			POP DX
			POP CX
			POP BX
			POP AX
			RET
CLS ENDP

DISP_SEC PROC NEAR	;�������ʾ����
			PUSH AX
			PUSH BX
			PUSH DX
	
			LEA BX, LED
			
			CALL CLOSE_LED;�ر������
	
			MOV DX, 0E488H
			MOV AL, 01H		;���ұ��������
			OUT DX, AL		;���λ��
		
			MOV DX, 0E48AH
			MOV AL, SEC
			AND AL, 0FH
			XLAT					;������Ӹ�λ����
			OUT DX, AL		;�������
	
			CALL CLOSE_LED;�ر������
	
			MOV DX, 0E488H
			MOV AL, 02H		;���ұ��������
			OUT DX, AL		;���λ��
	
			MOV DX, 0E48AH
			MOV AL, SEC

			PUSH CX
			MOV CL, 04H
			SHR AL, CL
			POP CX

			XLAT					;�������ʮλ����
			OUT DX, AL		;�������
	
			POP DX
			POP BX
			POP AX
			RET
DISP_SEC ENDP

INIT_8251 PROC NEAR	;��ʼ��8251
			PUSH AX
			PUSH DX
			
			MOV DX, 0E4B9H
		 	   
			MOV AL, 0
			OUT DX, AL
			OUT DX, AL
			OUT DX, AL
			
			MOV AL, 40H		;�ڲ���λ
			OUT DX, AL
			MOV AL, 4EH		;��ʽ�֣��첽��ʽ��1λֹͣλ��8λ����λ������żУ��λ������������16
			OUT DX, AL
			MOV AL, 27H		;�����֣������ͣ��������ͺͽ��գ������ն�׼����
			OUT DX, AL
   	     
			POP DX
			POP AX
			RET
INIT_8251 ENDP

INIT_8253 PROC NEAR	;��ʼ��8253
			PUSH AX
			PUSH DX
			
			MOV DX, 0E483H;������0����ʽ3�������Ƽ�����д���ֽ�
			MOV AL, 16H
			OUT DX, AL

			MOV DX, 0E480H
			MOV AL, 3AH		;������ֵ52
			OUT DX, AL
			
			POP DX
			POP AX
			RET
INIT_8253 ENDP

INIT_8255 PROC NEAR	;��ʼ��8255A
			PUSH AX
			PUSH DX
			
			MOV DX, 0E48BH;8255AоƬ����������� 
			MOV AL, 80H
			OUT DX, AL
			
			MOV DX, 0E4ABH;LED�����8255A�����������
			MOV AL, 80H
			OUT DX, AL
			
			POP DX
			POP AX
			RET
INIT_8255 ENDP

INIT_8259 PROC NEAR	;��ʼ��8259
			PUSH AX
			PUSH DX
			
			MOV AX, CS
			MOV DS, AX
			MOV DX, OFFSET IRQ	
			MOV AX, 250EH
			INT 21H				;���ж�����
			CLI
			
			MOV DX, 0EC4CH
			MOV AL, 43H		;���ж�
			OUT DX, AL
			
			INC DX
			MOV AL, 1DH		;����жϱ�־
			OUT DX, AL
			
			IN AL, 21H
			AND AL, 0BFH	;��IRQ6�ж�
			OUT 21H, AL
			
			MOV AX, DATA
			MOV DS, AX
			STI
			
			POP DX
			POP AX
			RET
INIT_8259 ENDP

INIT_CLOCK PROC NEAR;��ʼ��ʱ�ӵ�8253
			PUSH AX
			PUSH DX
			
			MOV SEC, 60H	;ʱ��60��
			
			MOV DX, 0E483H;������1����ʽ3�������Ƽ�������д���ֽ���д���ֽ�
			MOV AL, 76H
			OUT DX, AL

			MOV DX, 0E481H
			MOV AL, 0E8H	;1000��ʮ�����Ƶ��ֽ�
			OUT DX, AL
			
			MOV DX, 0E481H
			MOV AL, 03H		;1000��ʮ�����Ƹ��ֽ�
			OUT DX, AL
			
			MOV DX, 0E483H;������2����ʽ3�������Ƽ�������д���ֽ���д���ֽ�
			MOV AL, 0B6H
			OUT DX, AL

			MOV DX, 0E482H
			MOV AL, 0E8H
			OUT DX, AL
			
			MOV DX, 0E482H
			MOV AL, 03H
			OUT DX, AL
			
			MOV DI, 00H		;δ�����ж�

			POP DX
			POP AX
			RET
INIT_CLOCK ENDP

INPUT PROC NEAR			;����
			CALL INIT_CLOCK;��ʼ��ʱ��
			
			CMP CL, ME		;�Ƚ��Ƿ�Ϊ������
			JZ IN_ME
			CALL INPUT_ENEMY;������
			RET
IN_ME:CALL INPUT_ME	;�Լ���
			RET
INPUT ENDP

INPUT_ENEMY PROC NEAR;������
			CALL RECEIVE	;���ý��պ���
			CALL CLEAR_KEY;������̻���
			CALL PUT_CHESS;����
			RET
INPUT_ENEMY ENDP

INPUT_ME PROC NEAR	;�Լ���
			PUSH AX
			PUSH BX
			PUSH SI
			
LO_INPUT_ME1:
			PRINTS PLAYER_TIP;�����ʾ
			PRINTC CL			;����Լ�������ʽ
			PRINTC 3AH		;���ð��
			
			PUSH CX
			MOV CX, 02H		;������λʮ����������
			MOV DX, 00H		;����ĳ�ʼΪ0
LO_INPUT_ME2:
			MOV DH, DL		;����8λ�Ƶ���8λ
LO_INPUT_ME3:
			CALL SHOW_TAB	;�ڵ������ʾ����
			CALL DISP_SEC	;���������ʾ����ʱ
			CMP DI, 01H		;����Ƿ����ж�
			JNZ NOIRQ_INPUT_ME
			
			MOV AL, SEC		;���Ӽ�һ��BCD����
			DEC AL
			DAS
			MOV SEC, AL
			
			MOV DI, 00H		;����ж�
			
			CMP SEC, 10H	;�Ƿ��ǵ���10��
			JA NOBEEP_INPUT_ME
			CALL BEEP			;���屨��
			
NOBEEP_INPUT_ME:			
			CMP SEC, 00H	;�Ƿ�ʱ
			JNZ NOIRQ_INPUT_ME
			JMP TIMEOUT_INPUT_ME;������ʱ
NOIRQ_INPUT_ME:
			MOV AH, 01H		;�����̻������Ƿ�����������
			INT 16H
			JZ LO_INPUT_ME3
			
			MOV AH, 07H		;��������
			INT 21H
			
			CMP AL, 1BH		;�Ƿ�ΪESC
			JNZ NEXT_INPUT_ME0
			JMP GIVE_UP_INPUT_ME;ESC��Ȩ
NEXT_INPUT_ME0:			
			CMP AL, 08H		;�Ƿ�Ϊ�˸�
			JZ BKSP_INPUT_ME
			
			CMP AL, 31H		;�Ƿ�С��'1'
			JB LO_INPUT_ME3
			
			MOV BL, AL
			
			SUB AL, 30H		;��ȥ'0'
			CMP AL, 0AH		;�Ƿ�С��0AH
			JB NEXT_INPUT_ME1
			
			SUB AL, 07H
			JB LO_INPUT_ME3
			
			CMP AL, 0AH		;�Ƿ����0AH
			JB LO_INPUT_ME3
			
			CMP AL, 10H		;�Ƿ�С��10H����д��ĸ��
			JB NEXT_INPUT_ME1
			
			SUB AL, 20H		;��ȥ20H
			JB LO_INPUT_ME3
			
			CMP AL, 0AH		;�Ƿ����0AH
			JB LO_INPUT_ME3
			
			CMP AL, 0FH		;�Ƿ�С��10H��Сд��ĸ��
			JA LO_INPUT_ME3
			JMP NEXT_INPUT_ME1

BKSP_INPUT_ME:			;�˸�
			CMP CX, 02H		;�Ƿ�δ����
			JZ LO_INPUT_ME3
			
			PRINTC AL
			PRINTC ' '
			PRINTC AL			;�˸񸲸ǵ�ԭ��
			
			MOV DX, 00H		;���λ��
			MOV CX, 02H		;����������Ϊ0
			JMP LO_INPUT_ME3

NEXT_INPUT_ME1:
			MOV DL, AL		;���������λ��
			PRINTC BL			;��ӡ�ַ�
			
			DEC CX				;������һ
			JZ NEXT_INPUT_ME2
			JMP LO_INPUT_ME2
			
NEXT_INPUT_ME2:			
			POP CX
			PRINTLN				;����
			
			MOV AX, DX
			ADD AX, 0404H
			CALL XLAT_2D
			
			CMP BYTE PTR [SI], 'X';����Ƿ�Ҫ�µĵط�������
			JZ NEXT_INPUT_ME3
			CMP BYTE PTR [SI], 'O';����Ƿ�Ҫ�µĵط�������
			JZ NEXT_INPUT_ME3
			
			JMP RET_INPUT_ME
NEXT_INPUT_ME3:
			PRINTS WARNING;�������򾯸�
			JMP LO_INPUT_ME1

TIMEOUT_INPUT_ME:		;��ʱ
			PRINTS TIMEOUT
GIVE_UP_INPUT_ME:		;��Ȩ
			POP CX
			MOV DX, 00H
			
RET_INPUT_ME:
			POP SI
			POP BX
			POP AX
			
			CALL PUT_CHESS;����
			RET
INPUT_ME ENDP

MUSIC PROC NEAR			;���Ż�����
			PUSH SI
			MOV SI, 00H		;��һ��������ʼ
LO_MUSIC:
			CALL PLAY			;����
			
			MOV AH, 01H		;����а��������������
			INT 16H
			JNZ RET_MUSIC
			
			ADD SI, 02H		;ƫ�Ƶ�ַ��2
			
			CMP SI, 80H		;ֱ������
			JB LO_MUSIC
RET_MUSIC:
			POP SI
			RET
MUSIC ENDP

PLAY PROC NEAR			;����һ������
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH DX
			
			MOV AL, 0B6H	;��������8253������2����ʽ3��������λ��������д���ֽ���д���ֽ�
			OUT 43H, AL	
			
			MOV AX, 34DDH
			MOV BX, FREQ[SI];��ȡ����Ƶ��
			MOV DX, 12H
			DIV BX				;��1193181Hz��������Ƶ��
			
			OUT 42H, AL		;������ֽ�
			MOV AL, AH
			OUT 42H, AL		;������ֽ�

			IN AL, 61H		;�ݴ�΢��8255A��B������
			MOV BL, AL
			OR AL, 03H		;����8255A��B0,B1��Ϊ1
			OUT 61H, AL

			MOV AH, 86H		;��ʱָ��ʱ��
			MOV CX, TIME[SI]
			MOV DX, 00H
			INT 15H
			
			MOV AL, BL		;���ݴ��ԭ΢��8255A֮B�����ݻָ�
			OUT 61H, AL
			
			POP DX
			POP CX
			POP BX
			POP AX
			RET
PLAY ENDP

PRINT_CHAR PROC NEAR;��ӡ�ַ�
			PUSH AX
			MOV AH, 02H
			INT 21H				;���DL�洢�ַ�
			POP AX
			RET
PRINT_CHAR ENDP

PRINT_DIGIT PROC NEAR;��ӡʮ����������
			PUSH DX
			MOV DL, AL
			CALL TO_ASC		;��DL������תΪASCII��
			CALL PRINT_CHAR;��ӡ�ַ�
			POP DX
			RET
PRINT_DIGIT ENDP

PRINT_ENDL PROC NEAR;����
			PUSH AX
			PUSH DX
			MOV AH, 02H
			
			MOV DL, 0DH		;�س���
			INT 21H
			
			MOV DL, 0AH		;���з�
			INT 21H
			
			POP DX
			POP AX
			RET
PRINT_ENDL ENDP

PRINT_STR PROC NEAR	;��ӡ�ַ�
			PUSH AX
			MOV AH, 09H		;��ӡDX�е��ַ���
			INT 21H
			POP AX
			RET			
PRINT_STR ENDP

PRINT_TAB PROC NEAR	;��ӡ����
			PUSH AX
			PUSH CX
			PUSH DX
			PUSH SI
			
			CALL CLS			;����
			PRINTS TOP		;��ӡ����
			
			MOV AL, 00H
			LEA SI, TAB
			ADD SI, 57H
LO_TAB0:
			INC AL
			ADD SI, 08H
			CALL PRINT_DIGIT;��ӡ�к�
			
			MOV CX, 0FH
LO_TAB1:
			INC SI
			
			CMP CX, 0FH
			JNZ NEXT_TAB0
			
			PRINTC ' '
			JMP NEXT_TAB2
NEXT_TAB0:					;�����Ʊ���
			CMP AL, 01H
			JZ NEXT_TAB1
			CMP AL, 0FH
			JZ NEXT_TAB1
			
			PRINTC 0C4H
			JMP NEXT_TAB2
NEXT_TAB1:
			PRINTC 0CDH
NEXT_TAB2:			
			PRINTC [SI]		;��ӡ��������
			
			LOOP LO_TAB1
			
			PRINTLN				;����
			CMP AL, 0FH		;һ��ʮ����
			JNZ LO_TAB0
			
			POP SI
			POP DX
			POP CX
			POP AX
			RET
PRINT_TAB ENDP

PUT_CHESS PROC NEAR	;����
			CMP CL, ME
			JNZ DO_NOT_SEND
			
			CALL SEND			;�������Ӻ�Ҫ����
DO_NOT_SEND:
			CMP DX, 00H		;�Ƿ���Ȩ
			JZ GIVE_UP_PUT_CHESS	
			
			INC COUNT			;������һ
			
			PUSH AX
			PUSH SI
			
			MOV AX, DX
			ADD AX, 0404H
			CALL XLAT_2D
			MOV [SI], CL	;������ӷ���ָ��λ��
			
			POP SI
			POP AX
			
			CALL PRINT_TAB;�������
			
			PRINTS PLAYER_TIP;��������ʾ
			PRINTC CL			;������������ʽ
			PRINTC 3AH		;���ð��
			PRINTD DH			;��������к�
			PRINTD DL			;��������к�
			PRINTLN				;�������
			
			CALL CHECK_X	;������
			CMP LINE, 00H
			JNZ ENDGAME
			
			CALL CHECK_Y	;�������
			CMP LINE, 00H
			JNZ ENDGAME
			
			CALL CHECK_XY	;���б��
			CMP LINE, 00H
			JNZ ENDGAME
			
			CALL CHECK_X_Y;��鷴б��
			CMP LINE, 00H
			JNZ ENDGAME			
			
			JMP NOT_ENDGAME

ENDGAME:			
			CMP ME, CL		;�Ƿ�Ϊ�Լ�
			JZ WIN				;����ʤ
			JMP LOSE			;����
			
GIVE_UP_PUT_CHESS:	;��Ȩ
			PRINTLN
			PRINTC CL
			PRINTS GIVE_UP;�����ʾ
			
			CMP ME, CL		;�Ƿ�Ϊ�Լ�
			JNZ WIN				;����
			JMP LOSE			;����ʤ

WIN:	PRINTS YOU_WIN
			MOV STOP, 02H	;���ý���
			JMP RET_PUT_CHESS

LOSE:	PRINTS YOU_LOSE
			MOV STOP, 01H	;���ý���
			JMP RET_PUT_CHESS

NOT_ENDGAME:
			CMP COUNT, 0E1H;�Ƿ���225����
			JNZ RET_PUT_CHESS
			
			MOV STOP, 01H	;����
			PRINTS DRAW_GAME

RET_PUT_CHESS:
			RET
PUT_CHESS ENDP

READ_CHAR PROC NEAR	;��ȡ����������AL
			PUSH BX
			MOV BH, AH
			
			MOV AH, 01H
			INT 21H
			
			MOV AH, BH
			POP BX
			RET
READ_CHAR ENDP

RECEIVE PROC NEAR		;��ѯ��ʽ����
			PUSH AX        
			MOV DX, 0E4B9H
LO_RECEIVE:
			CALL SHOW_TAB	;�ڵ������ʾ����
			CALL DISP_SEC	;���������ʾ����ʱ
			CMP DI, 01H		;�Ƿ��ж�
			JNZ NOIRQ_RECEIVE
			
			MOV AL, SEC		;���Ӽ�һ��BCD����
      DEC AL
			DAS
      MOV SEC, AL
			
			MOV DI, 00H		;����ж�
			
			CMP SEC, 00H	;�Ƿ�ʱ
			JZ TIMEOUT_RECEIVE
NOIRQ_RECEIVE:
			IN AL, DX
			TEST AL, 02H
			JZ LO_RECEIVE	;����Ƿ���ܾ���
        
			MOV DX, 0E4B8H
			IN AL, DX			;��������
      
			MOV DH, AL		;����λ���ֻ�ԭΪʮ��λ����
			MOV DL, AL

      PUSH CX
      MOV CL, 04H
      SHR DH, CL
      POP CX

      AND DL, 0FH
        
      POP AX
      RET
TIMEOUT_RECEIVE:
			PRINTS TIMEOUT;��ӡ��ʱ
			MOV DX, 00H		;������Ϊ��Ȩ��־
			
			POP AX
			RET
RECEIVE ENDP

SELECT PROC NEAR		;ѡ���Ⱥ���
			PUSH AX
			
			PRINTS WELCOME
LO_SELECT:
			CALL READ_CHAR
			PRINTLN
			
			CMP AL, 'Y'
			JZ FIRST
			CMP AL, 'y'
			JZ FIRST			;����
			
			CMP AL, 'N'
			JZ SECOND
			CMP AL, 'n'
			JZ SECOND			;����
			
			PRINTS WARNING;���������
			JMP LO_SELECT
FIRST:MOV ME, 'X'		;��������Ϊ'X'
			POP AX
			RET
SECOND:
			MOV ME, 'O'		;��������Ϊ'O'
			POP AX
			RET
SELECT ENDP

SEND PROC NEAR
			PUSH AX
			PUSH DX
        
			MOV DX, 0E4B9H
LO_SEND:IN AL, DX
			TEST AL, 01H	;���ͻ������Ƿ�Ϊ��
			JZ LO_SEND
                        
			POP DX

			MOV AL, DH		;��ʮ��λ����ѹ��Ϊ��λ����
			PUSH CX
			MOV CL, 04H
			SHL AL, CL
			POP CX

			ADD AL, DL

			PUSH DX  
			MOV DX, 0E4B8H
			OUT DX, AL		;����λ��
        
			POP DX
			POP AX
			RET
SEND ENDP

SHOW_TAB PROC NEAR	;�������ʾ����
			PUSH AX
			PUSH BX
			PUSH CX
			PUSH DX
			PUSH SI
			
			MOV AL, 05H		;�����Ͻǿ�ʼ����
LO_SHOW_TAB1:
			MOV AH, 05H
			CALL CLOSE_LED;��LED
			
			MOV DX, 0E4AAH
			SUB AL, 05H
			OUT DX, AL		;����к�
			ADD AL, 05H
			
			MOV BX, 00H
LO_SHOW_TAB2:
			SHL BX, 01H		;BX����һλ
			CALL XLAT_2D
			
			CMP BYTE PTR [SI], 'X'
			JZ SHOW_CHESS1;����ΪX����ʾ�ڵ������
			CMP BYTE PTR [SI], 'O'
			JNZ NOT_SHOW_CHESS1
			CMP MAT_COUNT, 00H;����ΪO��MATCOUNT����Ϊ0����ʾ�ڵ������
			JNZ NOT_SHOW_CHESS1
SHOW_CHESS1:
			INC BX
NOT_SHOW_CHESS1:
			INC AH
			CMP AH, 0DH		;ǰ����
			JB LO_SHOW_TAB2
			
			PUSH AX
			MOV DX, 0E4A9H;���Ƶ�����ϰ���
			MOV AL, BL
			OUT DX, AL
			POP AX
			
			MOV BX, 00H
LO_SHOW_TAB3:
			SHL BX, 01H		;BX����һλ
			CALL XLAT_2D
			
			CMP BYTE PTR [SI], 'X'
			JZ SHOW_CHESS2;����ΪX����ʾ�ڵ������
			CMP BYTE PTR [SI], 'O'
			JNZ NOT_SHOW_CHESS2
			CMP MAT_COUNT, 00H
			JNZ NOT_SHOW_CHESS2;����ΪO��MATCOUNT����Ϊ0����ʾ�ڵ������
			
SHOW_CHESS2:
			INC BX
NOT_SHOW_CHESS2:
			INC AH
			CMP AH, 14H		;������
			JB LO_SHOW_TAB3
			
			PUSH AX
			MOV DX, 0E4A8H;���Ƶ�����°���
			MOV AL, BL
			SHL AL, 01H		;������һ��
			OUT DX, AL
			POP AX
			
			INC AL
			CMP AL, 14H		;�������15����
			JB LO_SHOW_TAB1
			
			INC MAT_COUNT
			AND MAT_COUNT, 07H;MAT_COUNT��0~7ѭ��
			
			POP SI
			POP DX
			POP CX
			POP BX
			POP AX
			RET
SHOW_TAB ENDP

TO_ASC PROC NEAR		;ת��ASCII��
			ADD DL, 30H		;��'0'
			CMP DL, 39H		;����'9'?
			JBE RET_ASC
			ADD DL, 07H		;��07H	
RET_ASC:
			RET
TO_ASC ENDP

XLAT_2D PROC NEAR		;���ά���飬AHΪ�кţ�ALΪ�кţ����Ͻ�Ϊ0101H
			PUSH AX
			PUSH DX
			
			MOV DH, 00H
			MOV DL, AH		;�кż�һ
			DEC DL
			MULTIPLY DX, 17H;����23
			
			MOV AH, 00H
			ADD DX, AX		;���к�
			DEC DX				;��һ
			
			LEA SI, TAB		;���
			ADD SI, DX		;��ƫ��
			
			POP DX
			POP AX
			RET
XLAT_2D ENDP

IRQ PROC FAR				;�жϷ������
			PUSH AX
			PUSH DX
			
			MOV DI, 01H		;��ʾ���ж�
			
			MOV AL, 20H
			OUT 20H, AL
			
			MOV DX, 0EC4DH
			MOV AL, 1DH		;���PCI9052���жϱ�־
			OUT DX, AL
			
			POP DX
			POP AX
			IRET
IRQ ENDP	

CODE ENDS
	END START