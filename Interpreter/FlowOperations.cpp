#include "pch.h"
#include "Opcodes.h"
#include <vector>

OPCODE_DEFINITION(JMP)				{ stack->IP += context.operand - 1; }
OPCODE_DEFINITION(JMP_IF_TOP_ZERO)	{ if (stack->PEEK() == 0) stack->IP += context.operand - 1; }
