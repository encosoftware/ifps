class KeyWord:
    def __init__(self, topic, word):
        self.__topic = topic
        self.__words = word.split(' ')

    def __eq__(self, other):
        return any([x in other.words for x in self.words if x != self.__topic]) and \
               any([x in self.words for x in other.words if x != other.topic])

    def __repr__(self):
        return ("%s "*len(self.words) % (*self.words,))[:-1]

    def __str__(self):
        return self.__repr__()

    def __len__(self):
        return len(self.words)

    @property
    def words(self):
        return self.__words

    @property
    def topic(self):
        return self.__topic


def search_similar_keywords(key_word_list):
    is_in = []
    counter = {}
    for key_word in key_word_list:
        if key_word in is_in:
            counter[is_in.index(key_word)] += 1
        else:
            counter[len(is_in)] = 1
            is_in.append(key_word)

    return [(is_in[k], v) for k, v in counter.items()]
