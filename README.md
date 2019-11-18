# PrRuleEngine

## DQs
1. The rule engine implenets a parser to parse incoming data (to minimize dependencies). You can add rules via a function call or from add your rules to an excel file. The parser get's the data and divides a parsable signal into valid and unvalid signals according to specified rules. The use of a custom parser instead of NewtonsoftJson is a tradeoff to reduce dependencies.

2. The parser is implemented with a complexity of O(n2).

3. Improvement in parser complexity. 
   Add rules to the csv via a function too.
  
