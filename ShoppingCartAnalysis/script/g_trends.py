from pytrends.request import TrendReq
from Analysis.KeyWord.KeyWord import KeyWord, search_similar_keywords
from matplotlib import pyplot as plt


pytrend = TrendReq()
query_word = ['szekrény', 'asztal', 'szék', 'ágy', 'kanapé']
query_word2 = ['polc', 'bútor', 'konyhabútor', 'nappali', 'fürdőszoba']
query_word3 = ['fotel', "ebédlő", 'háló', 'mosdó', 'gardrób']

key_words = []
# for query in [query_word]:
for query in [query_word, query_word2, query_word3]:
    pytrend.build_payload(kw_list=query, geo='HU')
    related_queries_dict = pytrend.related_queries()

    for key in query:
        key_words.extend([KeyWord(key, word) for word in related_queries_dict[key]['top']['query'].tolist()])
        # key_words.extend([KeyWord(key, word) for word in related_queries_dict[key]['rising']['query'].tolist()])

search_result = sorted(search_similar_keywords(key_words), key=lambda x: x[1] + 1/len(x[0]), reverse=True)

print(len(search_result))
val = 1  # len(search_result)/len(key_words)
print(val)
exp = []
for index, kv in enumerate(search_result):
    k, v = kv
    exp.append(1/((1+index)**val))
    print("%s :: %s --> %s" % (k, v, exp[-1]))

plt.plot(exp)
plt.show()

print("*"*100)
