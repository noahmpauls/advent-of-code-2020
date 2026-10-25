const SUBJECT: u64 = 7;
const DIVISOR: u64 = 20201227;

pub fn find_loop_size(public: u64) -> u64 {
    let mut key = 1;
    let mut loop_iters = 0;
    loop {
        if key != public {
            key = transform(key, SUBJECT);
            loop_iters += 1;
        } else {
            break;
        }
    }
    loop_iters
}

pub fn transform(num: u64, subject: u64) -> u64 {
    (num * subject) % DIVISOR
}