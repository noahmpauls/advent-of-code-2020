use std::collections::HashMap;
use regex::Regex;

type RuleId = usize;

#[derive(Debug)]
pub enum Rule {
    Char { 
        id: RuleId, 
        char: char 
    },
    SubRule { 
        id: RuleId, 
        rule: Vec<RuleId>, 
        or: Option<Vec<RuleId>>,
    },
}

impl Rule {
    pub fn from(rule: &str) -> Option<Rule> {
        let regex = r#"^(?P<id>[\d]+):\s*(?:(?:\s*"(?P<char>[a-z])")|(?:(?P<left>(?:\s*[\d])+)(?:\s*\|(?P<right>(?:\s*[\d])+))?))$"#;
        let regex = Regex::new(regex).unwrap();
        
        if let Some(captures) = regex.captures(rule) {
            let id = captures.name("id").unwrap().as_str().parse::<usize>().unwrap();
            if let Some(char) = captures.name("char") {
                Some(Rule::Char {
                    id,
                    char: char.as_str().chars().next().unwrap(),
                })
            } else if let Some(left) = captures.name("left") {
                let left: Vec<usize> = left.as_str()
                    .split_whitespace()
                    .map(|n| { n.parse::<usize>().unwrap() })
                    .collect();
                let right = match captures.name("right") {
                    Some(m) => {
                        let right: Vec<usize> = m.as_str()
                            .split_whitespace()
                            .map(|n| { n.parse::<usize>().unwrap() })
                            .collect();
                        Some(right)
                    },
                    None => None,
                };
                Some(Rule::SubRule {
                    id,
                    rule: left,
                    or: right,
                })
            } else {
                None
            }
        } else {
            None
        }
    }
}

pub struct RuleSet {
    rules: HashMap<RuleId, Rule>,
}

impl RuleSet {
    pub fn new() -> RuleSet {
        RuleSet {
            rules: HashMap::new(),
        }
    }

    pub fn as_regex_string(&self) -> String {
        format!("^{}$", self.rule_as_regex_string(0))
    }

    fn rule_as_regex_string(&self, rule: RuleId) -> String {
        match &self.rules[&rule] {
            Rule::Char { char: c, .. } => {
                String::from(*c)
            },
            Rule::SubRule { rule: left, or: right, .. } => {
                let mut result = String::from("(?:");
                for r in left.iter() {
                   result.push_str(&format!("({})", self.rule_as_regex_string(*r)));
                }
                result.push(')');
                if let Some(right) = right {
                    result.push_str("|(?:");
                    for r in right.iter() {
                        result.push_str(&format!("(?:{})", self.rule_as_regex_string(*r)));
                    }
                    result.push(')');
                }
                result
            },
        }

    }

    pub fn min_length(&self) -> usize {
        self.rule_min_length(0)
    }

    fn rule_min_length(&self, rule: RuleId) -> usize {
        match &self.rules[&rule] {
            Rule::Char { .. } => 1,
            Rule::SubRule { rule: left, or: right, .. } => {
                let mut min_length = left.iter().map(|r| { self.rule_min_length(*r) }).sum();
                if let Some(right) = right {
                    min_length = std::cmp::min(min_length, right.iter().map(|r| { self.rule_min_length(*r) }).sum());
                }
                min_length
            }
        }
    }

    pub fn insert_rule(&mut self, rule: Rule) {
        let id = match rule {
            Rule::Char{ id, .. } => id,
            Rule::SubRule { id, .. } => id,
        };
        self.rules.insert(id, rule);
    }

    pub fn insert_rule_from(&mut self, rule: &str) -> bool {
        match Rule::from(rule) {
            Some(rule) => {
                self.insert_rule(rule);
                true
            },
            None => false,
        }
    }
}

//##############################################################################
// TESTS
//##############################################################################

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_example1() {
        /*
        0: 1 2
        1: "a"
        2: 1 3 | 3 1
        3: "b"
        */

        let mut ruleset = RuleSet::new();
        ruleset.insert_rule(Rule::SubRule {
            id: 0,
            rule: vec![1, 2],
            or: None,
        });
        ruleset.insert_rule(Rule::Char {
            id: 1,
            char: 'a',
        });
        ruleset.insert_rule(Rule::SubRule {
            id: 2,
            rule: vec![1, 3],
            or: Some(vec![3, 1]),
        });
        ruleset.insert_rule(Rule::Char {
            id: 3,
            char: 'b',
        });

        let regex = ruleset.as_regex_string();

        println!("{}", regex);
        let regex = Regex::new(&regex).unwrap();
        let possibles = vec![
            ("aaba", false),
            ("bab", false),
            ("aba", true),
            ("aab", true),
            ("bbb", false),
        ];

        for (string, expected) in possibles {
            let actual = regex.is_match(string);
            assert_eq!(expected, actual,
                "regex.match({}) was {}, should have been {}",
                string, actual, expected);
        }
    }

    #[test]
    fn test_example2() {
        /*
        0: 4 1 5
        1: 2 3 | 3 2
        2: 4 4 | 5 5
        3: 4 5 | 5 4
        4: "a"
        5: "b"
        */

        let mut ruleset = RuleSet::new();
        ruleset.insert_rule(Rule::SubRule {
            id: 0,
            rule: vec![4, 1, 5],
            or: None,
        });

        ruleset.insert_rule(Rule::SubRule {
            id: 1,
            rule: vec![2, 3],
            or: Some(vec![3, 2]),
        });
        ruleset.insert_rule(Rule::SubRule {
            id: 2,
            rule: vec![4, 4],
            or: Some(vec![5, 5]),
        });
        ruleset.insert_rule(Rule::SubRule {
            id: 3,
            rule: vec![4, 5],
            or: Some(vec![5, 4]),
        });

        ruleset.insert_rule(Rule::Char {
            id: 4,
            char: 'a',
        });
        ruleset.insert_rule(Rule::Char {
            id: 5,
            char: 'b',
        });

        let regex = ruleset.as_regex_string();

        println!("{}", regex);
        let regex = Regex::new(&regex).unwrap();
        let possibles = vec!(
            ("ababbb", true),
            ("bababa", false),
            ("abbbab", true),
            ("aaabbb", false),
            ("aaaabbb", false),
        );

        for (string, expected) in possibles {
            let actual = regex.is_match(string);
            assert_eq!(expected, actual,
                "regex.match({}) was {}, should have been {}",
                string, actual, expected);
        }
    }
}